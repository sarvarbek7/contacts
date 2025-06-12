using Application.Common;
using Contacts.Application.Handlers.Messages.Handbooks;
using Contacts.Domain.Handbook;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IHandbookHandler
{
    public Task<ListResult<Handbook>> HandleListHandbooks(ListHandbookMessage message,
                                                          CancellationToken cancellationToken = default);

    public Task<ErrorOr<Handbook>> HandleGetHandleById(int Id,
                                                       CancellationToken cancellationToken = default);

    public Task<ErrorOr<Handbook>> HandleCreateHandbook(CreateHandbookMessage message,
                                                       CancellationToken cancellationToken = default);

    public Task<ErrorOr<Handbook>> HandleUpdateHandbook(UpdateHandbookMessage message,
                                                       CancellationToken cancellationToken = default);

    public Task<ErrorOr<Success>> HandleDeleteHandbook(DeleteHandbookMessage message,
                                                       CancellationToken cancellationToken = default);

    public Task<ErrorOr<HandbookItem>> HandleCreateHandbookItem(CreateHandlebookItemMessage message,
                                                           CancellationToken cancellationToken = default);

    public Task<ErrorOr<HandbookItem>> HandleUpdateHandbookItem(UpdateHandbookItemMessage message,
                                                           CancellationToken cancellationToken = default);

    public Task<ErrorOr<Success>> HandleDeleteHandbookItem(DeleteHandbookItemMessage message,
                                                           CancellationToken cancellationToken = default);
}