using Application.Common;
using Contacts.Application.Handlers.Messages.Accounts;
using Contacts.Domain.Accounts;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IAccountHandler
{
    public Task<ListResult<Account>> HandleListAccount(ListAccountMessage message, CancellationToken cancellationToken = default);

    public Task<ErrorOr<Account>> HandleCreateAccount(CreateAccountMessage message,
                                                      CancellationToken cancellationToken = default);

}