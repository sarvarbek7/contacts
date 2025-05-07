using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.PhoneNumbers;

public record ListPhoneNumbersQuery : PagedRecordQuery
{
    [FromQuery(Name = "search")]
    public string? Search { get; init; }
}