using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.PhoneNumbers;

public record ListPhoneNumbersQuery : PagedRecordQuery
{
    [FromQuery(Name = "number")]
    public string? Number { get; init; }

    [FromQuery(Name = "user")]
    public string? User { get; init; }
}