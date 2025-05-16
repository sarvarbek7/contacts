using Application.Services.Foundations;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Contacts.Domain.PhoneNumbers;

namespace Contacts.Infrastructure.Handlers;

internal class HrmHandler(IHrmProClient httpClient,
                          IHrmProcessingService hrmProcessingService) : IHrmHandler
{
    public async Task<ResponseWrapper<HrmListResponse<PositionWithPhoneNumber>>> GetPositionsWithPhoneNumbers(string queryParams, CancellationToken cancellationToken = default)
    {
        var login = await httpClient.Login(cancellationToken);

        var positionsResponse = await httpClient.GetPositions(login.TokenValue, queryParams, cancellationToken);

        var positions = positionsResponse.Data.Data;

        var positionWithPhoneNumbers = await hrmProcessingService.GetPositionWithPhoneNumbers(positions, cancellationToken);

        return new ResponseWrapper<HrmListResponse<PositionWithPhoneNumber>>()
        {
            Message = positionsResponse.Message,
            Error = positionsResponse.Error,
            Data = new HrmListResponse<PositionWithPhoneNumber>()
            {
                Page = positionsResponse.Data.Page,
                Total = positionsResponse.Data.Total,
                Data = positionWithPhoneNumbers
            }
        };
    }

    public async Task<ResponseWrapper<HrmListResponse<WorkerWithPhoneNumber>>> GetWorkersWithPhoneNumbers(string queryParams, CancellationToken cancellationToken = default)
    {
        var login = await httpClient.Login(cancellationToken);

        var workersResponse = await httpClient.GetWorkers(login.TokenValue, queryParams, cancellationToken);

        var workers = workersResponse.Data.Data;

        var workerWithPhoneNumbers = await hrmProcessingService.GetWorkersWithPhoneNumber(workers,
                                                                                          cancellationToken);

        return new ResponseWrapper<HrmListResponse<WorkerWithPhoneNumber>>()
        {
            Message = workersResponse.Message,
            Error = workersResponse.Error,
            Data = new HrmListResponse<WorkerWithPhoneNumber>()
            {
                Page = workersResponse.Data.Page,
                Total = workersResponse.Data.Total,
                Data = workerWithPhoneNumbers
            }
        };
    }
}
