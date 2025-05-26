using Contacts.Domain.PhoneNumbers;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record CreatePhoneNumberMessage(string Number, PhoneNumberType Type, int UserAccountIdWhoDoesAction) : IAuditableMessage
{
    public PhoneNumber MapToPhoneNumber()
    {
        return new PhoneNumber()
        {
            Number = Number,
            Type = Type,
        };
    }
}