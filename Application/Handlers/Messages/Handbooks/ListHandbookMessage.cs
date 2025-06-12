using Application.Common;

namespace Contacts.Application.Handlers.Messages.Handbooks;

public record ListHandbookMessage(Pagination Pagination,
                                  string? Search);