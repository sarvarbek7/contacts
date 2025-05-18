namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record RemovePositionUserPhoneNumberMessage(Guid Id,
                                           int UserAccountIdWhoDoesAction) : IAuditableMessage;
