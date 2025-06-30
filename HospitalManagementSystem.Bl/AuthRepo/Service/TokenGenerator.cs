using HospitalManagementSystem.Bl.AuthRepo.IService;
using HospitalManagementSystem.Db.Model.AuthModel;
using HospitalManagementSystem.Dto.AuthDto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AuthRepo.Service
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public TokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser user, IList<string> roles)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
