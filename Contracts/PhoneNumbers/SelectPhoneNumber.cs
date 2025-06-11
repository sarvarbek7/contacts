using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record SelectPhoneNumber(Guid Id, string Number, string Type, UserListItem? User);