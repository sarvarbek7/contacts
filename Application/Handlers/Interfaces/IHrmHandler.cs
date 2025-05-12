using System.Dynamic;

namespace Contacts.Application.Handlers.Interfaces;

public interface IHrmHandler
{
    Task<ExpandoObject> GetWorkersWithPhoneNumbers(string queryParams, 
        CancellationToken cancellationToken = default);
}