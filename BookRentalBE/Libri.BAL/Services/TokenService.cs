
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.CustomUser;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Libri.BAL.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
        }

        public string GenerateAccessToken(IEnumerable<CurrentUser> users)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.NameId, users.FirstOrDefault()!.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, users.FirstOrDefault()!.Email),
                new(JwtRegisteredClaimNames.GivenName, users.FirstOrDefault()!.UserName)
            };

            foreach (var user in users)
            {
                claims.Add(new(ClaimTypes.Role, user.Role));
            }

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}