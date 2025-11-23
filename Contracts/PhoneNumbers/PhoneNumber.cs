using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record PhoneNumber
{
    public Guid Id { get; init; }
    public required string Number { get; set; }
}

public record PositionAssignment
{
    
}