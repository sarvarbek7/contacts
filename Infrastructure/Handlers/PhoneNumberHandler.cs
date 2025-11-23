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
                         ITranslationService translationService) : IPhoneNumberHandler
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
            .Include(x => x.AssignedUsers)
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.PhoneNumberId);
        }


        phoneNumber.AssignUser(user, message.UserAccountIdWhoDoesAction);

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
            query = query.Where(x =>
                x.AssignedPositions.Any(
                    ap => ap.Users.Any(u => u.ExternalId == userExternalId)));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        if (message.PositionId is { } positionId)
        {
            query = query.Where(x => x.AssignedPositions.Any(ap => ap.PositionId == positionId));
        }

        if (message.Status is { } status)
        {
            query = status switch
            {
                Status.NotAssigned => query.Where(x => !x.AssignedPositions.Any()),
                Status.AssignedToUser => query.Where(x => x.AssignedPositions.Any(ap => ap.Users.Any()) || x.AssignedUsers.Any()),
                Status.AssignedToPosition => query.Where(x => x.AssignedPositions.Any()),
                Status.AssignedToPosition | Status.AssignedToUser => query.Where(x => x.AssignedPositions.Any(ap => ap.Users.Any()) || x.AssignedUsers.Any()),
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

        var workerIds = data.SelectMany(x => x.PositionWithExternalIds.SelectMany(pwi => pwi.WorkerExternalIds))
                            .ToList();

        var positionIds = data.SelectMany(x => x.PositionWithExternalIds.Select(pwi => pwi.PositionId))
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
            var positionsWrapper = phoneNumber.PositionWithExternalIds.Select(pwi =>
                new PositionWrapper(
                    pwi.PositionAssignmentId,
                    positions.FirstOrDefault(p => p.Id == pwi.PositionId)!,
                    workers.Where(w => pwi.WorkerExternalIds.Contains(w.Id))
                )).ToList();

            var phoneNumberWithPosition = new PhoneNumberListItemWithPosition(phoneNumber.Id,
                                                                              phoneNumber.Number,
                                                                              phoneNumber.Type.ToString(),
                                                                              positionsWrapper);
            if (userFilter && !positionsWrapper.Any(pw => pw.Workers.Any()))
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
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.Id);
        }

        phoneNumber.UnAssignUser(message.UserAssignmentId, message.UserAccountIdWhoDoesAction);

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
                v.Type = message.Type;
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
            .Include(x => x.AssignedPositions)
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.PhoneNumberId);
        }

        phoneNumber.AssignPosition(message.PositionId,
        message.OrganizationId,
        message.DepartmentId,
        message.InnerPositionId,
        message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Success>> HandleRemovePhoneNumberFromPosition(RemovePositionPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.Id, tracked: true)
            .Include(x => x.AssignedPositions)
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.Id);
        }

        phoneNumber.UnAssignPosition(message.PositionAssignmentId, message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Success>> HandlePositionUserAssignPhoneNumber(AssignPositionUserPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrUser = await userHandler.HandleAddOrGetUser(message.User, cancellationToken);

        if (errorOrUser.IsError)
        {
            return errorOrUser.FirstError;
        }

        var user = errorOrUser.Value;

        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.PhoneNumberId,
                                tracked: true)
            .Include(x => x.AssignedPositions.Where(x => x.Id == message.PositionAssignmentId))
                .ThenInclude(x => x.Users)
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.PhoneNumberId);
        }

        var positionAssignment = phoneNumber.AssignedPositions
            .FirstOrDefault(x => x.Id == message.PositionAssignmentId);

        if (positionAssignment is null || positionAssignment.PositionId != message.PositionId)
        {
            return Application.Common.Errors.ApplicationErrors.PhoneNumberNotAssignedToPosition;
        }

        positionAssignment.AssignUser(user, message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Success>> HandleRemovePositionUserPhoneNumber(RemovePositionUserPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.Id, tracked: true)
                                                  .Include(x => x.AssignedPositions.Where(p => p.Id == message.PositionAssignmentId))
                                                    .ThenInclude(p => p.Users.Where(u => u.Id == message.UserId))
                                                  .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFoundForGivenId<PhoneNumber, Guid>(message.Id);
        }

        var positionAssignment = phoneNumber.AssignedPositions
            .FirstOrDefault(x => x.Id == message.PositionAssignmentId);

        if (positionAssignment is null)
        {
            return Application.Common.Errors.ApplicationErrors.PhoneNumberNotAssignedToPosition;
        }

        positionAssignment.UnAssignUser(message.UserId, message.UserAccountIdWhoDoesAction);

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

        string query = $"?organization_id={message.OrganizationId}&per_page={perPage}";

        if (message.PositionId.HasValue)
        {
            query += $"&department_position_id={message.PositionId}";
        }

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
        var workerIdsWhoHasPhoneNumber = await phoneNumberService.GetAll(x => x.AssignedPositions
                                                                             .Any(ap => ap.PositionId == message.PositionId &&
                                                                            ap.Users
                                                                         .Any(u => workerIds.Contains(u.ExternalId))))
            .SelectMany(x => x.AssignedPositions).SelectMany(ap => ap.Users).Select(u => u.ExternalId).ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var workersWhoHasPhoneNumber = workers.Where(x => workerIdsWhoHasPhoneNumber.Contains(x.Id)).ToList();

        var workersWithPhoneNumber = await hrmProcessingService.GetWorkersWithPhoneNumber(workersWhoHasPhoneNumber,
            cancellationToken);

        return workersWithPhoneNumber;
    }
}