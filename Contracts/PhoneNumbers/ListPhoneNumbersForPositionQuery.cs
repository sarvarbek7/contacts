using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.PhoneNumbers;

public record ListPhoneNumbersForPositionQuery
{
    [FromQuery(Name = "positionId")]
    public int PositionId { get; init; }

    [FromQuery(Name = "organizationId")]
    public int OrganizationId { get; init; }
}