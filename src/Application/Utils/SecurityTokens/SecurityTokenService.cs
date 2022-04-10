using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasyMed.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace EasyMed.Application.Utils.SecurityTokens;

public record AuthResponse(string Token, long ExpirationTimestamp, Role Role);

public interface ISecurityTokenService
{
    string GenerateAccessTokenForUser(int userId, string Email, Role role);
}

public class SecurityTokenService : ISecurityTokenService
{
    private readonly ApplicationSettings _applicationSettings;

    public SecurityTokenService(ApplicationSettings applicationSettings)
    {
        _applicationSettings = applicationSettings;
    }

    public string GenerateAccessTokenForUser(int userId, string email, Role role)
    {
        var claims = new ClaimsIdentity(new[]
        {
            new Claim("type", "access"), new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("role", role.ToString()), new Claim(ClaimTypes.Email, email)
        });

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSettings.AccessTokenSettings.Key));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.Now.AddMinutes(_applicationSettings.AccessTokenSettings.ExpiryTimeInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims, Expires = expiresAt, SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}