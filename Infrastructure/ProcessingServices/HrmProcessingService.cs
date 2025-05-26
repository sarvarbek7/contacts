using Application.Services.Foundations;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contacts.Infrastructure.ProcessingServices;

internal class HrmProcessingService(IBaseService<PhoneNumber, Guid> phoneNumberService,
    IPositionChangingNotifier notifier,
    ILogger<HrmProcessingService> logger) : IHrmProcessingService
{
    public async Task<List<PositionWithPhoneNumber>> GetPositionWithPhoneNumbers(List<Position> positions, CancellationToken cancellationToken = default)
    {
        var positionIds = positions.Select(x => x.Id).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (positionIds.Count > 0)
        {
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => positionIds.Contains(x.ActiveAssignedPositionId!.Value),
                                                                               tracked: false)
                .ToListAsync(cancellationToken);

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }

        List<PositionWithPhoneNumber> positionWithPhoneNumbers = [];

        foreach (var position in positions)
        {
            var userPhoneNumbers =
                phoneNumbers.Where(x => x.ActiveAssignedPositionId == position.Id)
                .Select(x => new PhoneNumberItem()
                {
                    Id = x.Id,
                    Number = x.Number,
                    Type = x.TypeString
                }).ToList();

            PositionWithPhoneNumber positionWithPhoneNumber = new()
            {
                Id = position.Id,
                PhoneNumbers = userPhoneNumbers,
                Organization = position.Organization,
                Department = position.Department,
                PositionItem = position.PositionItem
            };

            positionWithPhoneNumbers.Add(positionWithPhoneNumber);
        }

        return positionWithPhoneNumbers;
    }

    public async Task<List<WorkerWithPhoneNumber>> GetWorkersWithPhoneNumber(List<WorkerResponse> workers, CancellationToken cancellationToken = default)
    {
        var workerIds = workers.Select(x => x.Id).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (workerIds.Count > 0)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => workerIds.Contains(x.ActiveAssignedPositionUser.ExternalId),
                                                                               tracked: false)
                .Include(x => x.ActiveAssignedPositionUser)
                .ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }

        List<WorkerWithPhoneNumber> workerWithPhoneNumbers = [];

        foreach (var worker in workers)
        {
            var userPhones = phoneNumbers.Where(x => x.ActiveAssignedPositionUser!.ExternalId == worker.Id);

            var changePositionPhoneNumbers = userPhones.Where(x => x.ActiveAssignedPositionId != worker.DepartmentPosition.Id);

            foreach (var changePositionPhoneNumber in changePositionPhoneNumbers)
            {
                var positionChangedMessage = new PositionChangedMessage()
                {
                    PositionId = changePositionPhoneNumber.ActiveAssignedPositionId ?? 0,
                    UserExternalId = worker.Id
                };

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

            var userPhoneNumbers =
                userPhones.Where(x => x.ActiveAssignedPositionId == worker.DepartmentPosition.Id)
                .Select(x => new PhoneNumberItem()
                {
                    Id = x.Id,
                    Number = x.Number,
                    Type = x.TypeString
                }).ToList();

            WorkerWithPhoneNumber workerWithPhoneNumber = new()
            {
                Id = worker.Id,
                Organization = worker.Organization,
                Department = worker.Department,
                DepartmentPosition = worker.DepartmentPosition,
                Worker = worker.Worker,
                PhoneNumbers = userPhoneNumbers,
            };

            workerWithPhoneNumbers.Add(workerWithPhoneNumber);
        }

        return workerWithPhoneNumbers;
    }

    public async Task<List<WorkerWithPhoneNumber>> GetWorkersWithPhoneNumberInPosition(List<WorkerResponse> workers,
                                                                                       int? positionId,
                                                                                       CancellationToken cancellationToken = default)
    {
        var workerIds = workers.Select(x => x.Id).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (workerIds.Count > 0)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            IQueryable<PhoneNumber> query = phoneNumberService.GetAll(x => x.ActiveAssignedPositionUserId != null &&
                                                                               workerIds.Contains(x.ActiveAssignedPositionUser.ExternalId),
                                                                               tracked: false)
                                                    .Include(x => x.ActiveAssignedPositionUser);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            if (positionId.HasValue)
            {
                query = query.Where(x => x.ActiveAssignedPositionId == positionId.Value);
            }

            var storedPhoneNumbersForWorkers = await query
            .ToListAsync(cancellationToken);

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }

        List<WorkerWithPhoneNumber> workerWithPhoneNumbers = [];

        foreach (var worker in workers)
        {
            var userPhoneNumbers =
                phoneNumbers.Where(x => x.ActiveAssignedPositionUser!.ExternalId == worker.Id)
                .Select(x => new PhoneNumberItem()
                {
                    Id = x.Id,
                    Number = x.Number,
                    Type = x.TypeString
                }).ToList();

            WorkerWithPhoneNumber workerWithPhoneNumber = new()
            {
                Id = worker.Id,
                Organization = worker.Organization,
                Department = worker.Department,
                DepartmentPosition = worker.DepartmentPosition,
                Worker = worker.Worker,
                PhoneNumbers = userPhoneNumbers,
            };

            if (workerWithPhoneNumber.PhoneNumbers.Count > 0)
            {
                workerWithPhoneNumbers.Add(workerWithPhoneNumber);
            }
        }

        return workerWithPhoneNumbers;
    }
}