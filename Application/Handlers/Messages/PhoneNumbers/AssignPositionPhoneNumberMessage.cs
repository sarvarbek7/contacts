namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignPositionPhoneNumberMessage(int PositionId,
                                               Guid PhoneNumberId,
                                               int UserAccountIdWhoDoesAction) : IAuditableMessage;