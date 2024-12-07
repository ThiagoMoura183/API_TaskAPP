using Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.AuthService;
public class AuthService(IConfiguration configuration) : IAuthService {

    private readonly IConfiguration _configuration = configuration;
    public string GenerateJWT(string email, string username) {
        var issuer = _configuration["JWT:Issuer"];
        var audience = _configuration["JWT:Audience"];
        var key = _configuration["JWT:Key"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> {
            new("Email", email),
            new("Username", username),
            new("EmailIdentifier", email.Split("@").ToString()!),
            new("CurrentTime", DateTime.Now.ToString())
        };

        _ = int.TryParse(_configuration["JWT:TokenExpirationDays"], out int tokenExpirationDays);

        var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(tokenExpirationDays),
                signingCredentials: credentials
            );

        // O handler é responsável por manusear o token anteriormente gerado
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken() {
        var secureRandomBytes = new byte[128];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(secureRandomBytes);
        return Convert.ToBase64String(secureRandomBytes);
    }
}
