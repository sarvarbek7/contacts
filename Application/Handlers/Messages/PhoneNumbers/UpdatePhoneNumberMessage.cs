namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record UpdatePhoneNumberMessage(Guid Id,
                                       string Number,
                                       int UserAccountIdWhoDoesAction) : IAuditableMessage;