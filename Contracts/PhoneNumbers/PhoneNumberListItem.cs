using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record PhoneNumberListItem(Guid Id, string Number, UserListItem? AssignedUser);