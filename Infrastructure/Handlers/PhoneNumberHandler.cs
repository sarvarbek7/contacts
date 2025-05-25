using Application.Common;
using Application.Errors;
using Application.ProcessingServices;
using Application.Services.Foundations;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Contacts.Domain.PhoneNumbers;
using Contacts.Shared;
using ErrorOr;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Contacts.Infrastructure.Extensions;
using Application.Common.Extensions;
using Contacts.Application.ProcessingServices.Models;
using Microsoft.Extensions.Logging;

namespace Contacts.Infrastructure.Handlers;

class PhoneNumberHandler(IBaseService<PhoneNumber, Guid> phoneNumberService,
                         IUserHandler userHandler,
                         IHrmProClient hrmClient,
                         IHrmProcessingService hrmProcessingService,
                         ITranslationService translationService,
                         IPositionChangingNotifier notifier,
                         ILogger<PhoneNumberHandler> logger) : IPhoneNumberHandler
{
    public async Task<ErrorOr<Success>> HandleUserAssignPhoneNumber(AssignUserPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrUser = await userHandler.HandleAddOrGetUser(message.User, cancellationToken);

        if (errorOrUser.IsError)
        {
            return errorOrUser.FirstError;
        }

        var user = errorOrUser.Value;

        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.PhoneNumberId, tracked: true)
            .Include(x => x.ActiveAssignedUser)
            .Include(x => x.UsersHistory.Where(x => x.IsActive == true))
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.PhoneNumberId);
        }

        if (phoneNumber.ActiveAssignedUser is not null)
        {
            return Application.Common.Errors.ApplicationErrors.NumberAlreadyAssignedToUser;
        }

        phoneNumber.AssignUser(user, message.OrganizationId, message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Created>> HandleCreate(CreatePhoneNumberMessage message,
                             CancellationToken cancellationToken = default)
    {
        var phoneNumber = message.MapToPhoneNumber();

        phoneNumber.CreatedById = message.UserAccountIdWhoDoesAction;
        phoneNumber.CreatedAt = DateTime.UtcNow;

        bool phoneNumberExists = await phoneNumberService.GetAll(x => x.Number == phoneNumber.Number).AnyAsync(cancellationToken);

        if (phoneNumberExists)
        {
            return Application.Common.Errors.ApplicationErrors.PhoneNumberAlreadyExists;
        }

        return await phoneNumberService.Add(phoneNumber, true, cancellationToken);
    }

    public async Task<ErrorOr<Deleted>> HandleDelete(DeletePhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrStoredPhoneNumber = await phoneNumberService.GetById(message.Id,
                                                                 tracked: true,
                                                                 cancellationToken: cancellationToken);

        return await errorOrStoredPhoneNumber.MatchAsync<ErrorOr<Deleted>>(async v =>
            {
                v.DeletedById = message.UserAccountIdWhoDoesAction;
                v.DeletedAt = DateTime.UtcNow;

                return await phoneNumberService.Delete(v, true, cancellationToken);
            }, async e =>
            {
                await Task.CompletedTask;

                return e;
            });
    }

    public async Task<ErrorOr<PhoneNumber>> HandleGetById(GetPhoneNumberByIdMessage message, CancellationToken cancellationToken = default)
    {
        return await phoneNumberService.GetById(message.Id,
                                                tracked: true,
                                                includeStrings: [nameof(PhoneNumber.ActiveAssignedUser),
                                                $"{nameof(PhoneNumber.UsersHistory)}.{nameof(UserPhoneNumber.User)}"],
                                                cancellationToken: cancellationToken);

    }

    public async Task<ListResult<PhoneNumberListItemWithPosition>> HandleList(ListPhoneNumbersMessage message, CancellationToken cancellationToken = default)
    {
        var query = phoneNumberService.GetAll(tracked: false).AsExpandable();

        if (message.Number?.Trim().ToLower() is { } number)
        {
            query = query.Where(x => EF.Functions.ILike(x.Number, $"%{number}%"));
        }

        if (message.UserExternalId is { } userExternalId)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            query = query.Where(x => x.ActiveAssignedPositionUser.ExternalId == userExternalId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        if (message.PositionId is { } positionId)
        {
            query = query.Where(x => x.ActiveAssignedPositionId == positionId);
        }

        if (message.Positions.Count > 0)
        {
            query = query.Where(x => message.Positions.Contains(x.ActiveAssignedPositionId!.Value));
        }

        if (message.Status is { } status)
        {
            query = status switch
            {
                Status.NotAssigned => query.Where(x => x.ActiveAssignedPositionId == null && x.ActiveAssignedPositionUserId == null),
                Status.AssignedToUser => query.Where(x => x.ActiveAssignedPositionUser != null),
                Status.AssignedToPosition => query.Where(x => x.ActiveAssignedPositionUser == null && x.ActiveAssignedPositionId != null),
                Status.AssignedToPosition | Status.AssignedToUser => query.Where(x => x.ActiveAssignedPositionId != null || x.ActiveAssignedPositionId == null),
                _ => query,
            };
        }

        bool userFilter = false;

        if (message.User?.Trim().ToLower() is { } user)
        {
            userFilter = true;

            var names = user.Split(' ', StringSplitOptions.RemoveEmptyEntries |
                                        StringSplitOptions.TrimEntries);

            var predicate = message.BuildPredicate(translationService);

            if (predicate.IsStarted)
            {
                query = query.Where(predicate);
            }
        }

        var total = await query.CountAsync(cancellationToken);

        query = query.Paged(message.Pagination);

        var data = await query.Select(PhoneNumberListItem.To.Expand()).ToListAsync(cancellationToken);

        var workerIds = data.Where(x => x.AssignedUser is not null)
                            .Select(x => x.AssignedUser!.ExternalId)
                            .ToList();

        var positionIds = data.Where(x => x.PositionId is not null)
                              .Select(x => x.PositionId)
                              .ToList();


        string workersQuery = $"?per_page={workerIds.Count}&ids={string.Join(',', workerIds)}";
        string positionsQuery = $"?per_page={positionIds.Count}&ids={string.Join(',', positionIds)}";

        var login = await hrmClient.Login(cancellationToken);

        List<Position> positions = [];
        bool hasPosition = true;
        int positionPage = 1;

        while (hasPosition && positionIds.Count > 0)
        {
            string queryWithPage = positionsQuery + $"&page={positionPage}";

            var response = await hrmClient.GetPositions(login.TokenValue, queryWithPage, cancellationToken);

            var listResponse = response.Data;

            if (listResponse.Total > positionPage * positionIds.Count)
            {
                positionPage++;
            }
            else
            {
                hasPosition = false;
            }

            positions.AddRange(listResponse.Data);
        }

        List<WorkerResponse> workers = [];
        bool hasWorker = true;
        int workerPage = 1;

        while (hasWorker && workerIds.Count > 0)
        {
            string queryWithPage = workersQuery + $"&page={workerPage}";

            var response = await hrmClient.GetWorkers(login.TokenValue, queryWithPage, cancellationToken);

            var listResponse = response.Data;

            if (listResponse.Total > positionPage * workerIds.Count)
            {
                workerPage++;
            }
            else
            {
                hasWorker = false;
            }

            workers.AddRange(listResponse.Data);
        }

        List<PhoneNumberListItemWithPosition> phoneNumberListItemWithPositions = [];

        foreach (var phoneNumber in data)
        {
            WorkerResponse? worker = null;


            if (phoneNumber.AssignedUser is not null)
            {
                worker = workers.FirstOrDefault(x => x.Id == phoneNumber.AssignedUser.ExternalId);

                if (worker is not null && worker.DepartmentPosition.Id != phoneNumber.PositionId)
                {
                    var positionChangedMessage = new PositionChangedMessage()
                    {
                        PositionId = phoneNumber.PositionId ?? 0,
                        UserExternalId = worker.Id
                    };

                    worker = null;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Run(() =>
                    {
                        try
                        {
                            notifier.Notify(positionChangedMessage);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Error occuring while notify position change");
                        }
                    }, CancellationToken.None);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }

            Position? position = null;

            if (phoneNumber.PositionId != null)
            {
                position = positions.FirstOrDefault(x => x.Id == phoneNumber.PositionId);
            }

            var phoneNumberWithPosition = new PhoneNumberListItemWithPosition(phoneNumber.Id,
                                                                              phoneNumber.Number,
                                                                              worker,
                                                                              position);
            if (userFilter && worker is null)
            {
            }
            else
            {
                phoneNumberListItemWithPositions.Add(phoneNumberWithPosition);
            }
        }

        return message.ToListResultWithData(phoneNumberListItemWithPositions, total);
    }

    public async Task<ErrorOr<Success>> HandleRemoveUserPhoneNumber(RemoveUserPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.Id, tracked: true)
            .Include(x => x.UsersHistory.Where(x => x.IsActive == true))
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.Id);
        }

        phoneNumber.UnAssignUser(message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Updated>> HandleUpdate(UpdatePhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrStoredPhoneNumber = await phoneNumberService.GetById(message.Id,
                                                                 tracked: true,
                                                                 cancellationToken: cancellationToken);

        return await errorOrStoredPhoneNumber.MatchAsync<ErrorOr<Updated>>(async v =>
            {
                v.Number = message.Number;
                v.UpdatedById = message.UserAccountIdWhoDoesAction;
                v.UpdatedAt = DateTime.UtcNow;

                return await phoneNumberService.Update(v, true, cancellationToken);
            }, async e =>
            {
                await Task.CompletedTask;

                return e;
            });
    }

    public async Task<ErrorOr<Success>> HandlePositionAssignPhoneNumber(AssignPositionPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.PhoneNumberId, tracked: true)
            .Include(x => x.PositionHistory.Where(x => x.IsActive == true))
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.PhoneNumberId);
        }

        phoneNumber.AssignPosition(message.PositionId,
        message.OrganizationId,
        message.Organization,
        message.Department,
        message.Position,
        message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Success>> HandleRemovePhoneNumberFromPosition(RemovePositionPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.Id, tracked: true)
            .Include(x => x.PositionHistory.Where(x => x.IsActive == true))
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.Id);
        }

        phoneNumber.UnAssignPosition(message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<List<WorkerWithPhoneNumber>> HandlePositionPhoneNumbers(ListPhoneNumbersForPositionMessage message,
                                                                        CancellationToken cancellationToken = default)
    {
        var login = await hrmClient.Login(cancellationToken);

        List<WorkerResponse> workers = [];
        bool hasWorker = true;
        int page = 1;
        int perPage = 100;

        string query = $"?organization_id={message.OrganizationId}&department_position_id={message.PositionId}&per_page={perPage}";

        while (hasWorker)
        {
            string queryWithPage = query + $"&page={page}";

            var response = await hrmClient.GetWorkers(login.TokenValue, queryWithPage, cancellationToken);

            var listResponse = response.Data;

            if (listResponse.Total > page * perPage)
            {
                page++;
            }
            else
            {
                hasWorker = false;
            }

            workers.AddRange(listResponse.Data);
        }

        var workerIds = workers.Select(x => x.Id).ToList();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var workerIdsWhoHasPhoneNumber = await phoneNumberService.GetAll(x => workerIds.Contains(x.ActiveAssignedPositionUser.ExternalId))
            .Select(x => x.ActiveAssignedPositionUser.ExternalId).ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var workersWhoHasPhoneNumber = workers.Where(x => workerIdsWhoHasPhoneNumber.Contains(x.Id)).ToList();

        var workersWithPhoneNumber = await hrmProcessingService.GetWorkersWithPhoneNumber(workersWhoHasPhoneNumber,
            cancellationToken);

        return workersWithPhoneNumber;
    }

    public async Task<ListResult<PhoneNumberListItem>> HandleSearchByUser(SearchPhoneNumbersByUserMessage message, CancellationToken cancellationToken = default)
    {
        IQueryable<PhoneNumber> query = phoneNumberService.GetAll(x => x.ActiveAssignedUserId != null,
                                                                  tracked: false)
                                      ;

        var predicate = message.BuildPredicate(translationService);

        if (predicate.IsStarted)
        {
            query = query.Where(predicate);
        }

        int total = await query.CountAsync(cancellationToken);

        query = query.Paged(message.Pagination);

        var data = await query.AsExpandable().Select(PhoneNumberListItem.To).ToListAsync(cancellationToken);


        return message.ToListResultWithData(data,
                                            total);
    }

    public async Task<ErrorOr<Success>> HandlePositionUserAssignPhoneNumber(AssignPositionUserPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrUser = await userHandler.HandleAddOrGetUser(message.User, cancellationToken);

        if (errorOrUser.IsError)
        {
            return errorOrUser.FirstError;
        }

        var user = errorOrUser.Value;

        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.PhoneNumberId, tracked: true)
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.PhoneNumberId);
        }

        if (phoneNumber.ActiveAssignedPositionId != message.PositionId)
        {
            return Application.Common.Errors.ApplicationErrors.PhoneNumberNotAssignedToPosition;
        }

        phoneNumber.AssignPositionUser(user, message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<List<WorkerWithPhoneNumber>> HandlePositionPhoneNumbersClient(ListPhoneNumbersForPositionMessageClient message, CancellationToken cancellationToken = default)
    {
        var login = await hrmClient.Login(cancellationToken);

        List<WorkerResponse> workers = [];
        bool hasWorker = true;
        int page = 1;
        int perPage = 100;

        string query = $"?organization_id={message.OrganizationId}&department_position_id={message.PositionId}&per_page={perPage}";

        while (hasWorker)
        {
            string queryWithPage = query + $"&page={page}";

            var response = await hrmClient.GetWorkers(login.TokenValue, queryWithPage, cancellationToken);

            var listResponse = response.Data;

            if (listResponse.Total > page * perPage)
            {
                page++;
            }
            else
            {
                hasWorker = false;
            }

            workers.AddRange(listResponse.Data);
        }

        var workerIds = workers.Select(x => x.Id).ToList();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var workerIdsWhoHasPhoneNumber = await phoneNumberService.GetAll(x => x.ActiveAssignedPositionUserId != null &&
                                                                         workerIds.Contains(x.ActiveAssignedPositionUser.ExternalId))
            .Select(x => x.ActiveAssignedPositionUser.ExternalId).ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var workersWhoHasPhoneNumber = workers.Where(x => workerIdsWhoHasPhoneNumber.Contains(x.Id)).ToList();

        var workersWithPhoneNumber = await hrmProcessingService.GetWorkersWithPhoneNumberInPosition(workersWhoHasPhoneNumber,
            message.PositionId,
             cancellationToken);

        return workersWithPhoneNumber;
    }

    public async Task<ErrorOr<Success>> HandleRemovePositionUserPhoneNumber(RemovePositionUserPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.Id, tracked: true)
            .Include(x => x.UsersHistory.Where(x => x.IsActive == true))
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.Id);
        }

        phoneNumber.UnAssignPositionUser(message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }
}