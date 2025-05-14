using System.Dynamic;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.ProcessingServices;

public interface IHrmProClient
{
    public Task<LoginResponse> Login(CancellationToken cancellationToken = default);

    public Task<ResponseWrapper<List<Organization>>> GetStructure(string token,
                                         CancellationToken cancellationToken = default);

    public Task<ResponseWrapper<List<Department>>> GetDepartments(string token,  string query, CancellationToken cancellationToken = default);
    public Task<ResponseWrapper<ListResponse<Position>>> GetPositions(string token, string query, CancellationToken cancellationToken = default);
    public Task<ResponseWrapper<ListResponse<WorkerResponse>>> GetWorkers(string token, string query, CancellationToken cancellationToken = default);
}