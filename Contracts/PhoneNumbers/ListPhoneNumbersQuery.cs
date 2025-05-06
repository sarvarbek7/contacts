using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.PhoneNumbers;

public record ListPhoneNumbersQuery([FromQuery] string? Name,
                               [FromQuery] int? PositionId,
                               [FromQuery] string? Number,
                               [FromQuery] string? Search);