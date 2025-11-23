namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record RemovePositionUserPhoneNumberMessage(Guid Id,
                                                   Guid PositionAssignmentId,
                                                   int UserId,
                                                   int UserAccountIdWhoDoesAction) : IAuditableMessage;
