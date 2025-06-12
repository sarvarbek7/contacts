using Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.Handbooks;

public record ListHandbooksRequest : PagedRecordQuery
{
    [FromQuery(Name = "search")]
    public string? Search { get; init; }
}