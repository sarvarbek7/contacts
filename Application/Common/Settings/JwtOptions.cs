namespace Contacts.Application.Common.Settings;

public record JwtOptions
{
    public string AccessTokenSecretKey { get; init; } = null!;
    public int AccessTokenValidInMinutes { get; init; }
    public string RefreshTokenSecretKey { get; init; } = null!;
    public int RefreshTokenValidInHours { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;

    public const string SectionName = "Jwt";
}