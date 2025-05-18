using Application.Common;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record SelectPhoneNumberMessage(int? PositionId, Pagination Pagination) : IPaginatedMessage;
