namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record DeletePhoneNumberMessage(Guid Id, int UserAccountIdWhoDoesAction) : IAuditableMessage;
