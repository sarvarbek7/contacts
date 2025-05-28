using Contacts.Application.ProcessingServices.Models.Requests.HrmPro;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.ProcessingServices;

public interface IHrmProClient
{
    public Task<LoginResponse> Login(CancellationToken cancellationToken = default);

    public Task<ResponseWrapper<List<Organization>>> GetStructure(string token,
                                         CancellationToken cancellationToken = default);

    public Task<ResponseWrapper<List<Department>>> GetDepartments(string token, int organizationId, CancellationToken cancellationToken = default);
    public Task<ResponseWrapper<HrmListResponse<Position>>> GetPositions(string token, ListPositionsQuery query, CancellationToken cancellationToken = default);
    public Task<ResponseWrapper<HrmListResponse<Position>>> GetPositions(string token, string query, CancellationToken cancellationToken = default);
    public Task<ResponseWrapper<HrmListResponse<WorkerResponse>>> GetWorkers(string token, string query, CancellationToken cancellationToken = default);
    public Task<ResponseWrapper<HrmListResponse<DepartmentSearchItem>>> GetSearchDepartments(string token, string query, CancellationToken cancellationToken = default);
}