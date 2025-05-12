using Contacts.Domain.Users;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignUserPhoneNumberMessage(User User,
                                           Guid PhoneNumberId,
                                           int UserAccountIdWhoDoesAction) : IAuditableMessage;