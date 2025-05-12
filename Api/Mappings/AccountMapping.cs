using System.Linq.Expressions;
using Contacts.Application.Handlers.Messages.Accounts;
using Contacts.Contracts.Accounts;
using Contacts.Contracts.Auth;
using Contacts.Domain.Accounts;

namespace Contacts.Api.Mappings;

public static class AccountMapping
{
    public static Contracts.Accounts.Account MapTo(this Domain.Accounts.Account account)
    {
        return new Contracts.Accounts.Account()
        {
            Id = account.Id,
            Login = account.Login,
            Role = account.Role.ToString(),
        };
    }

    public static Expression<Func<Domain.Accounts.Account, Contracts.Accounts.Account>> AccountDomainToContract =>
        x => new Contracts.Accounts.Account()
        {
            Id = x.Id,
            Login = x.Login,
            Role = x.Role.ToString()
        };

    public static CreateAccountMessage MapTo(this CreateAccountRequest request, int accountId)
    {

        var role = Enum.Parse<Role>(request.Role, ignoreCase: true);

        return new(request.Login, request.Password, role, accountId);
    }

    public static ListAccountMessage MapTo(this ListAccountsQuery query)
    {
        return new(query.ToPagination());
    }
}