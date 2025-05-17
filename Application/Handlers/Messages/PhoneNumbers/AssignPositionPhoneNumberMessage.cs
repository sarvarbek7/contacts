namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignPositionPhoneNumberMessage(int PositionId,
                                               Guid PhoneNumberId,
                                               int OrganizationId,
                                               string Organization,
                                               string Department,
                                               string Position,
                                               int UserAccountIdWhoDoesAction) : IAuditableMessage;