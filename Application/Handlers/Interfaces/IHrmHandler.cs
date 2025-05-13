using System.Dynamic;

namespace Contacts.Application.Handlers.Interfaces;

public interface IHrmHandler
{
    Task<ExpandoObject> GetWorkersWithPhoneNumbers(string queryParams, 
        CancellationToken cancellationToken = default);
    
    Task<ExpandoObject> GetPositionsWithPhoneNumbers(string queryParams, 
        CancellationToken cancellationToken = default);
}