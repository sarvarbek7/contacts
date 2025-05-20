using Contacts.Shared;
using Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.PhoneNumbers;

public record ListPhoneNumbersQuery : PagedRecordQuery
{
    [FromQuery(Name = "status")]
    public Status? Status { get; init; }

    [FromQuery(Name = "number")]
    public string? Number { get; init; }

    [FromQuery(Name = "user")]
    public string? User { get; init; }

    [FromQuery(Name = "userExternalId")]
    public int? UserExternalId { get; init; }

    [FromQuery(Name = "positionId")]
    public int? PositionId { get; init; }

    [FromQuery(Name = "positions")]
    public string? Positions { get; init; }
}