using Application.Common;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record ListPhoneNumbersMessage(Pagination Pagination, string? Number, string? User);