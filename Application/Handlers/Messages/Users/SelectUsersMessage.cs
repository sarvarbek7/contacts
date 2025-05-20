using Application.Common;

namespace Contacts.Application.Handlers.Messages.Users;

public record SelectUsersMessage(string? Search,
                                 bool? HaveNumber,
                                 Pagination Pagination) : IPaginatedMessage;