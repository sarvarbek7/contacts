using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.ProcessingServices;

public interface IHrmProcessingService
{
    Task<List<WorkerWithPhoneNumber>> GetWorkersWithPhoneNumber(List<WorkerResponse> workers, CancellationToken cancellationToken = default);
    Task<List<WorkerWithPhoneNumber>> GetWorkersWithPhoneNumberInPosition(List<WorkerResponse> workers,
                                                                          int positionId,
                                                                          CancellationToken cancellationToken = default);
    Task<List<PositionWithPhoneNumber>> GetPositionWithPhoneNumbers(List<Position> positions, CancellationToken cancellationToken = default);
    
}