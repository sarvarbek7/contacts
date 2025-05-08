namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record RemovePhoneNumberMessage(Guid Id,
                                       int UserAccountIdWhoDoesAction) : IAuditableMessage;
