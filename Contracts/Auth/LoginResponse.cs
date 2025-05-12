using Contacts.Contracts.Accounts;

namespace Contacts.Contracts.Auth;
public record LoginResponse
{
    public required string AccessToken { get; init; }
    public DateTime AccessTokenExpiresAt { get; init; }
    public required string RefreshToken { get; init; }
    public DateTime RefreshTokenExpiresAt { get; init; }
    public required Account Account { get; init; }
}