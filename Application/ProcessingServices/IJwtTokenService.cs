using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Contacts.Application.ProcessingServices.Models;

namespace Contacts.Application.ProcessingServices;

public interface IJwtTokenService
{
    string RenewToken(JwtSecurityToken token, List<Claim>? extraClaims = null);
    AccessTokenResponse GenerateAccessToken(int accountId, List<Claim>? extraClaims = null);
    RefreshTokenResponse GenerateRefreshToken(int accountId);
    public JwtSecurityToken ReadToken(string token);
}