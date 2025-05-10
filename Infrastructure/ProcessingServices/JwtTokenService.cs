using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contacts.Application.Common.Settings;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Contacts.Infrastructure.ProcessingServices;

internal class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    readonly JwtOptions jwtOptions = options.Value ?? 
        throw new InvalidOperationException("Jwt options not found");

    public JwtSecurityToken ReadToken(string token)
    {
        return new JwtSecurityTokenHandler().ReadJwtToken(token);
    }

    public AccessTokenResponse GenerateAccessToken(int accountId, List<Claim>? extraClaims = null)
    {
        var now = DateTime.Now;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, accountId.ToString()),
        };

        if (extraClaims != null)
        {
            claims.AddRange(extraClaims);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.AccessTokenSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = now.AddMinutes(jwtOptions.AccessTokenValidInMinutes);

        var token = new JwtSecurityToken(
        notBefore: now,
        expires: expiresAt,
        claims: claims,
        signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AccessTokenResponse(accessToken, expiresAt);
    }

    public RefreshTokenResponse GenerateRefreshToken(int accountId)
    {
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, accountId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.RefreshTokenSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = now.AddHours(jwtOptions.RefreshTokenValidInHours);

        var token = new JwtSecurityToken(
        claims: claims,
        notBefore: now,
        expires: expiresAt,
        signingCredentials: creds
        );

        var refreshToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new RefreshTokenResponse(refreshToken, expiresAt);
    }

    public string RenewToken(JwtSecurityToken token, List<Claim>? extraClaims = null)
    {
        var now = DateTime.UtcNow;

        List<Claim> claims = [.. token.Claims];

        if (extraClaims is not null)
        {
            claims.AddRange(extraClaims);
        }

        var expires = now.AddMinutes(jwtOptions.AccessTokenValidInMinutes);
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.AccessTokenSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var newToken = new JwtSecurityToken(
        claims: claims,
        notBefore: now,
        expires: expires,
        signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(newToken);
    }
}
