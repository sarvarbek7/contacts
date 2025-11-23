using Contacts.Domain.Users;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignPositionUserPhoneNumberMessage(User User,
                                                   Guid PositionAssignmentId,
                                                   int PositionId,
                                                   Guid PhoneNumberId,
                                                   int UserAccountIdWhoDoesAction) : IAuditableMessage;