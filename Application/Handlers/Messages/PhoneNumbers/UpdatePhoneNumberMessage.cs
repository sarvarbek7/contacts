using Contacts.Domain.PhoneNumbers;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record UpdatePhoneNumberMessage(Guid Id,
                                       string Number,
                                       PhoneNumberType Type,
                                       int UserAccountIdWhoDoesAction) : IAuditableMessage;