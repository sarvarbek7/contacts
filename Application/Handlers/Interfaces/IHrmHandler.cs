using System.Dynamic;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.Handlers.Interfaces;

public interface IHrmHandler
{
    Task<ResponseWrapper<ListResponse<WorkerWithPhoneNumber>>> GetWorkersWithPhoneNumbers(string queryParams, 
        CancellationToken cancellationToken = default);
    
    Task<ResponseWrapper<ListResponse<PositionWithPhoneNumber>>> GetPositionsWithPhoneNumbers(string queryParams, 
        CancellationToken cancellationToken = default);
}