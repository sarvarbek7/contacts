using Contacts.Domain.Users;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignPhoneNumberMessage(User User, Guid PhoneNumberId, int UserAccountIdWhoDoesAction) : IAuditableMessage
{
}
