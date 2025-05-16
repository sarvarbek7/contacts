using Contacts.Shared;
using Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.PhoneNumbers;

public record ListPhoneNumbersByUserQuery : PagedRecordQuery
{
    [FromQuery(Name = "status")]
    public Status? Status { get; init; }

    [FromQuery(Name = "user")]
    public string? User { get; init; }
}