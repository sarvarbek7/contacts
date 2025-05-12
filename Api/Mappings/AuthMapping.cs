using Contacts.Application.Handlers.Messages.Accounts;
using Contacts.Application.Handlers.Messages.Auth;
using Contacts.Application.ProcessingServices.Models.Responses.Auth;
using Contacts.Contracts.Auth;
using Contacts.Domain.Accounts;

namespace Contacts.Api.Mappings;

public static class AuthMapping
{
    public static LoginMessage MapTo(this LoginRequest request) =>
        new(request.Login, request.Password);

    public static Contracts.Auth.LoginResponse MapTo(this Application.ProcessingServices.Models.Responses.Auth.LoginResponse response)
    {
        return new Contracts.Auth.LoginResponse()
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            AccessTokenExpiresAt = response.AccessTokenExpiresAt,
            RefreshTokenExpiresAt = response.RefreshTokenExpiresAt,
            Account = response.Account.MapTo()
        };
    }
}