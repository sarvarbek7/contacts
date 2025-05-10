using Contacts.Domain.Accounts;

namespace Contacts.Application.ProcessingServices.Models.Responses.Auth;

public record LoginResponse
{
    public LoginResponse(AccessTokenResponse accessToken,
        RefreshTokenResponse refreshToken,
        Account account)
    {
        AccessToken = accessToken.Value;
        AccessTokenExpiresAt = accessToken.ExpireAt;
        RefreshToken = refreshToken.Value;
        RefreshTokenExpiresAt = refreshToken.ExpireAt;
        Account = account;
    }

    public string AccessToken { get; init; }
    public DateTime AccessTokenExpiresAt { get; init; }
    public string RefreshToken { get; init; }
    public DateTime RefreshTokenExpiresAt { get; init; }
    public Account Account { get; init; }
}