using DigiDock.Base.Token;
using DigiDock.Data.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigiDock.Business.Token
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig jwtConfig;

        public TokenService(JwtConfig jwtConfig)
        {
            this.jwtConfig = jwtConfig ?? throw new ArgumentNullException(nameof(jwtConfig));
        }

        public async Task<string> GetToken(User user)
        {
            return await GenerateToken(user);
        }

        public async Task<string> GenerateToken(User user)
        {
            Claim[] claims = GetClaims(user);
            var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }

        private Claim[] GetClaims(User user)
        {
            List<Claim> claims = new List<Claim>()
        {
            new Claim("UserName", user.FirstName + " " + user.LastName),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Role", user.Role),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

            return claims.ToArray();
        }
    }
}
