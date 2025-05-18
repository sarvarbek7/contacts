using Application.Common;
using Contacts.Shared;

namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record ListPhoneNumbersMessage(Pagination Pagination,
                                      string? Number,
                                      string? User,
                                      string? PositionUser,
                                      Status? Status,
                                      int? UserExternalId,
                                      int? PositionId,
                                      List<int> Positions) : IUserNameSearchMessage, IPaginatedMessage;