using ClashFlow.Domain.Entities;
using ClashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Infrastructure.Security.Tokens
{
    class JwtTokenGenerator (
        uint expirationTimeInMinutes,
        string secretKey
    ) : IAccessTokenGenerator
    {
        private readonly uint _expirationTimeInMinutes = expirationTimeInMinutes;
        private readonly string _secretKey = secretKey;
        public string Generate(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims),
            };

            var TokenHandler = new JwtSecurityTokenHandler();

            var securityToken = TokenHandler.CreateToken(tokenDescriptor);

            return TokenHandler.WriteToken(securityToken);
        }

        private SymmetricSecurityKey SecurityKey()
        {
            var key = Encoding.UTF8.GetBytes(_secretKey);
            return new SymmetricSecurityKey(key);
        }
    }
}
