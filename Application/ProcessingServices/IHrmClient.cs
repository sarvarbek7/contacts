using Contacts.Application.ProcessingServices.Models;
using Contacts.Application.ProcessingServices.Models.Responses.Hrm;
using ErrorOr;

namespace Contacts.Application.ProcessingServices;

public interface IHrmClient
{
    ValueTask<LoginResponse> Login(CancellationToken cancellationToken= default);

    ValueTask<ErrorOr<Worker>> GetWorkerByPinfl(string pinfl,
                                                string token,
                                                CancellationToken cancellationToken = default);

    ValueTask<List<OrganizationStructure>> GetStructure(string token,
                                                        CancellationToken cancellationToken = default);
}