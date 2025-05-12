namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record RemoveUserPhoneNumberMessage(Guid Id,
                                           int UserAccountIdWhoDoesAction) : IAuditableMessage;
