using Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.PhoneNumbers;

public class SelectPhoneNumbersQuery : PagedClassQuery
{
    [FromQuery(Name = "position_id")]
    public int? PositionId { get; init; }

    [FromQuery(Name = "search")]
    public string? Search { get; init; }
}