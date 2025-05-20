using Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.Users;

public record SelectQuery : PagedRecordQuery
{
    [FromQuery(Name = "search")]
    public string? Search { get; init; }

    [FromQuery(Name = "haveNumber")]
    public bool? HaveNumber { get; init; }
}