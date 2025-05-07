using System.Dynamic;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.ProcessingServices;

public interface IHrmProClient
{
    public Task<LoginResponse> Login(CancellationToken cancellationToken = default);

    public Task<ExpandoObject> GetStructure(string token,
                                         CancellationToken cancellationToken = default);

    public Task<ExpandoObject> GetDepartments(string token,  string query, CancellationToken cancellationToken = default);
    public Task<ExpandoObject> GetPositions(string token, string query, CancellationToken cancellationToken = default);
    public Task<ExpandoObject> GetWorkers(string token, string query, CancellationToken cancellationToken = default);
}