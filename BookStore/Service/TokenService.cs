
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using black_follow.Entity;
using Microsoft.IdentityModel.Tokens;


namespace BookStore.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user , string role);
        
    }
  
        public class TokenService : ITokenService
        {
            private readonly SymmetricSecurityKey _key;

            public TokenService(IConfiguration config)
            {
                _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            }

            public string CreateToken(AppUser user, string role)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    // add role Claim
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("Role", role),

                    // new Claim(JwtRegisteredClaimNames., user.Email.ToString()),
                };

                var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(30),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }

      
        }
    
}