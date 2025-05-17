using Application.Services.Foundations;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.ProcessingServices;

internal class HrmProcessingService(IBaseService<PhoneNumber, Guid> phoneNumberService) : IHrmProcessingService
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
                    Number = x.Number
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
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => workerIds.Contains(x.ActiveAssignedUser.ExternalId),
                                                                               tracked: false)
                .Include(x => x.ActiveAssignedUser)
                .ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }

        List<WorkerWithPhoneNumber> workerWithPhoneNumbers = [];

        foreach (var worker in workers)
        {
            var userPhoneNumbers =
                phoneNumbers.Where(x => x.ActiveAssignedUser!.ExternalId == worker.Id)
                .Select(x => new PhoneNumberItem()
                {
                    Id = x.Id,
                    Number = x.Number,
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
                                                                                       int positionId,
                                                                                       CancellationToken cancellationToken = default)
    {
        var workerIds = workers.Select(x => x.Id).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (workerIds.Count > 0)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => x.ActiveAssignedPositionId == positionId && x.ActiveAssignedPositionUserId != null &&
                                                                               workerIds.Contains(x.ActiveAssignedPositionUser.ExternalId),
                                                                               tracked: false)
                                                    .Include(x => x.ActiveAssignedPositionUser)
                .ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

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