using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record PhoneNumberListItemSearchUser
{
    public Guid Id{get; init;}
    public required string Number {get; init;}
    public required string Type { get; init; }
}