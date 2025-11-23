namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record RemovePositionPhoneNumberMessage(Guid Id,
                                               Guid PositionAssignmentId,
                                               int UserAccountIdWhoDoesAction) : IAuditableMessage;
