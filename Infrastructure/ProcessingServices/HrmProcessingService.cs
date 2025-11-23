using Application.Services.Foundations;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contacts.Infrastructure.ProcessingServices;

internal class HrmProcessingService(IBaseService<PhoneNumber, Guid> phoneNumberService) : IHrmProcessingService
{
    public async Task<List<PositionWithPhoneNumber>> GetPositionWithPhoneNumbers(List<Position> positions, CancellationToken cancellationToken = default)
    {
        var positionIds = positions.Select(x => x.Id).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (positionIds.Count > 0)
        {
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => x.AssignedPositions.Any(ap => positionIds.Contains(ap.PositionId)),
                                                                               tracked: false)
                .ToListAsync(cancellationToken);

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }

        List<PositionWithPhoneNumber> positionWithPhoneNumbers = [];

        foreach (var position in positions)
        {
            var userPhoneNumbers =
                phoneNumbers.Where(x => x.AssignedPositions.Any(ap => ap.PositionId == position.Id))
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
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => x.AssignedPositions.Any(
                                                                                    ap => ap.Users.Any(u => workerIds.Contains(u.ExternalId))),
                                                                               tracked: false)
                .Include(x => x.AssignedPositions)
                .ThenInclude(ap => ap.Users)
                .ToListAsync(cancellationToken);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }

        List<WorkerWithPhoneNumber> workerWithPhoneNumbers = [];

        foreach (var worker in workers)
        {
            var userPhones = phoneNumbers.Where(x => x.AssignedPositions.Any(
                                        ap => ap.Users.Any(u => u.ExternalId == worker.Id)))
                                        .ToList();


            var userPhoneNumbers =
                userPhones.Select(x => new PhoneNumberItem()
                {
                    Id = x.Id,
                    Number = x.Number,
                    Type = x.TypeString
                }).ToList();

            var workerWithPhoneNumber = WorkerWithPhoneNumber.MapFrom(worker, userPhoneNumbers);

            workerWithPhoneNumbers.Add(workerWithPhoneNumber);
        }

        return workerWithPhoneNumbers;
    }
}