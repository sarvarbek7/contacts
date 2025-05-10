using Contacts.Application.Handlers.Messages.Auth;
using Contacts.Contracts.Auth;
using Contacts.Domain.Accounts;

namespace Contacts.Api.Mappings;

public static class AuthMapping
{
    public static LoginMessage MapTo(this LoginRequest request) =>
        new(request.Login, request.Password);

    public static CreateAccountMessage MapTo(this CreateAccountRequest request, int accountId)
    {
        Role? role = null;

        if (request.Role is not null)
        {
            role = Enum.Parse<Role>(request.Role, ignoreCase: true);
        }

        return new(request.Login, request.Password, role, accountId);
    }
}