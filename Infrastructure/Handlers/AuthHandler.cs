using System.Security.Claims;
using Application.Services.Foundations;
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
    public async Task<ErrorOr<Account>> HandleCreateAccount(CreateAccountMessage message, CancellationToken cancellationToken = default)
    {
        var storedAccount = await accountService.GetAll(x => x.Login == message.Login)
           // TODO: Add error
           .SingleOrDefaultAsync(cancellationToken);

        if (storedAccount is not null)
        {
            // TODO: Account already exists error
            throw new NotImplementedException();
        }

        if (string.IsNullOrWhiteSpace(message.Password))
        {
            // TODO: Invalid password
            throw new NotImplementedException();
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

    public async Task<LoginResponse> HandleLogin(LoginMessage message,
        CancellationToken cancellationToken = default)
    {
        var storedAccount = await accountService.GetAll(x => x.Login == message.Login)
            // TODO: Add error
            .SingleOrDefaultAsync(cancellationToken) ?? throw new NotImplementedException();

        if (!passwordHashingService.VerifyPassword(message.Password, storedAccount.Password))
        {
            // TODO: Add error
            throw new NotImplementedException();
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
