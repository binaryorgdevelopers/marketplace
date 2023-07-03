using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;

namespace Identity.Infrastructure.Services;

internal class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public TokenProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public async ValueTask<JsonWebToken> GenerateToken(TokenRequest tokenRequest)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", tokenRequest.Id.ToString())
            }),
            Expires = expires,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new JsonWebToken(tokenHandler.WriteToken(token), expires.ToShortTimeString());
    }

    public Guid? ValidateJwtToken(string? token)
    {
        if (token is null) return null;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = "http://localhost:1111",
                ValidAudience = _jwtOptions.ValidAudience,
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "id").Value);
            return userId;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}

public interface ITokenProvider
{
    ValueTask<JsonWebToken> GenerateToken(TokenRequest tokenRequest);

    Guid? ValidateJwtToken(string? token);
}