using Application.Common;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record SearchPhoneNumbersByUserMessage(Pagination Pagination, string? User) : IUserNameSearchMessage, IPaginatedMessage;