using Application.Common;
using Application.Common.Extensions;
using Application.Services.Foundations;
using Contacts.Application.Common.Errors;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.Accounts;
using Contacts.Application.ProcessingServices;
using Contacts.Domain.Accounts;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

internal class AccountHandler(IBaseService<Account, int> accountService,
                              IPasswordHashingService passwordHashingService) : IAccountHandler
{
    public async Task<ErrorOr<Account>> HandleCreateAccount(CreateAccountMessage message, CancellationToken cancellationToken = default)
    {
        var storedAccount = await accountService.GetAll(x => x.Login == message.Login)
           .SingleOrDefaultAsync(cancellationToken);

        if (storedAccount is not null)
        {
            return ApplicationErrors.AccountExists;
        }

        if (message.Password is {Length: < 8})
        {
            return ApplicationErrors.InvalidPassword;
        }

        var passwordHash = passwordHashingService.GenerateHash(message.Password);

        var account = new Account()
        {
            Login = message.Login,
            Password = passwordHash,
            Role = message.Role,
            CreatedAt = DateTime.UtcNow,
            CreatedById = message.UserAccountIdWhoDoesAction
        };

        var addedResult = await accountService.Add(account, true, cancellationToken);

        if (addedResult.IsError)
        {
            return addedResult.FirstError;
        }

        return account;
    }

    public async Task<ListResult<Account>> HandleListAccount(ListAccountMessage message, CancellationToken cancellationToken = default)
    {
        var query = accountService.GetAll(tracked: false);

         var total = await query.CountAsync(cancellationToken);

        query = query.Paged(message.Pagination);

        return message.ToListResultWithQuery(query, total);

    }
}
