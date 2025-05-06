namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record DeletePhoneNumberMessage(Guid Id) : IAuditableMessage
{
    public int UserAccountIdWhoDoesAction { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
}
