using Contacts.Domain.Users;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignPositionUserPhoneNumberMessage(User User,
                                                   int PositionId,
                                                   Guid PhoneNumberId,
                                                   int UserAccountIdWhoDoesAction) : IAuditableMessage;