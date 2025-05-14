using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Application.Services.Foundations;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

internal class HrmHandler(IHrmProClient httpClient,
                          IBaseService<PhoneNumber, Guid> phoneNumberService) : IHrmHandler
{
    public async Task<ResponseWrapper<ListResponse<PositionWithPhoneNumber>>> GetPositionsWithPhoneNumbers(string queryParams, CancellationToken cancellationToken = default)
    {
        var login = await httpClient.Login(cancellationToken);

        var positionsResponse = await httpClient.GetPositions(login.TokenValue, queryParams, cancellationToken);

        var positions = positionsResponse.Data.Data;

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

        return new ResponseWrapper<ListResponse<PositionWithPhoneNumber>>()
        {
            Message = positionsResponse.Message,
            Error = positionsResponse.Error,
            Data = new ListResponse<PositionWithPhoneNumber>()
            {
                Page = positionsResponse.Data.Page,
                Total = positionsResponse.Data.Total,
                Data = positionWithPhoneNumbers
            }
        };
    }

    public async Task<ResponseWrapper<ListResponse<WorkerWithPhoneNumber>>> GetWorkersWithPhoneNumbers(string queryParams, CancellationToken cancellationToken = default)
    {
        var login = await httpClient.Login(cancellationToken);

        var workersResponse = await httpClient.GetWorkers(login.TokenValue, queryParams, cancellationToken);

        var workers = workersResponse.Data.Data;

        var workerIds = workers.Select(x => x.Worker.Id).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (workerIds.Count > 0)
        {
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => workerIds.Contains(x.ActiveAssignedUser.ExternalId),
                                                                               tracked: false)
                .Include(x => x.ActiveAssignedUser)
                .ToListAsync(cancellationToken);

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }

        List<WorkerWithPhoneNumber> workerWithPhoneNumbers = [];

        foreach (var worker in workers)
        {
            var userPhoneNumbers =
                phoneNumbers.Where(x => x.ActiveAssignedUser!.ExternalId == worker.Worker.Id)
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

        return new ResponseWrapper<ListResponse<WorkerWithPhoneNumber>>()
        {
            Message = workersResponse.Message,
            Error = workersResponse.Error,
            Data = new ListResponse<WorkerWithPhoneNumber>()
            {
                Page = workersResponse.Data.Page,
                Total = workersResponse.Data.Total,
                Data = workerWithPhoneNumbers
            }
        };
    }
}
