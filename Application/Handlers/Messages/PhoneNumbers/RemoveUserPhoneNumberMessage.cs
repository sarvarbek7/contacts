namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record RemoveUserPhoneNumberMessage(Guid Id,
                                           Guid UserAssignmentId,
                                           int UserAccountIdWhoDoesAction) : IAuditableMessage;
