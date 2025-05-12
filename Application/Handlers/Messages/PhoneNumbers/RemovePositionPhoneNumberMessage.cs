namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record RemovePositionPhoneNumberMessage(Guid Id,
                                           int UserAccountIdWhoDoesAction) : IAuditableMessage;
