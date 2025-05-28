using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.Handlers.Interfaces;

public interface IHrmHandler
{
    Task<ResponseWrapper<HrmListResponse<WorkerWithPhoneNumber>>> GetWorkersWithPhoneNumbers(string queryParams, 
        CancellationToken cancellationToken = default);
    
    Task<ResponseWrapper<HrmListResponse<PositionWithPhoneNumber>>> GetPositionsWithPhoneNumbers(string queryParams, 
        CancellationToken cancellationToken = default);
}