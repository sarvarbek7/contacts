using System.Security.Claims;
using Application.Services.Foundations;
using Contacts.Application.Common.Errors;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.Auth;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.Auth;
using Contacts.Domain.Accounts;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

internal class AuthHandler(IBaseService<Account, int> accountService,
                           IPasswordHashingService passwordHashingService,
                           IJwtTokenService jwtTokenService) : IAuthHandler
{
    public async Task<ErrorOr<LoginResponse>> HandleLogin(LoginMessage message,
        CancellationToken cancellationToken = default)
    {
        var storedAccount = await accountService.GetAll(x => x.Login == message.Login)
            .SingleOrDefaultAsync(cancellationToken);

        if (storedAccount is null)
        {
            return ApplicationErrors.InvalidCredentials;
        }

        if (!passwordHashingService.VerifyPassword(message.Password, storedAccount.Password))
        {
            return ApplicationErrors.InvalidCredentials;
        }

        List<Claim> claims = [];

        if (storedAccount.Role is { } role)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        var accessToken = jwtTokenService.GenerateAccessToken(storedAccount.Id, claims);
        var refreshToken = jwtTokenService.GenerateRefreshToken(storedAccount.Id);

        return new LoginResponse(accessToken, refreshToken, storedAccount);
    }
}
