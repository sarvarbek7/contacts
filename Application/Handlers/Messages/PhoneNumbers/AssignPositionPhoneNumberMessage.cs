namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignPositionPhoneNumberMessage(int PositionId,
                                               string Organization,
                                               string Department,
                                               string Position,
                                               Guid PhoneNumberId,
                                               int UserAccountIdWhoDoesAction) : IAuditableMessage;