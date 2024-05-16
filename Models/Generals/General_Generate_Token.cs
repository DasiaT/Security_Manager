using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Manager_Security_BackEnd.Models.Generals
{
    public class General_Generate_Token
    {
        private readonly conectionDBcontext _context;
        public General_Generate_Token(conectionDBcontext context)
        {
            _context = context;
        }

        public JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) => new(
           new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
           new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
           claims,
           expires: expiration,
           signingCredentials: credentials
        );

        public async Task<List<Claim>> CreateClaimsAsync(User_Login user)
        {
            var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];

            await ManejarTokenAnteriorAsync(user);

            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.User_Name),
                    new Claim(ClaimTypes.Email, user.Information_User.Email),
                    new Claim(ClaimTypes.Surname, user.Information_User.Surnames.ToString()),
                    new Claim(ClaimTypes.Expired, user.ExpiresInMinutes.ToString())
                };

                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public SigningCredentials CreateSigningCredentials()
        {
            var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];

            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)), SecurityAlgorithms.HmacSha256);
        }

        public async Task ManejarTokenAnteriorAsync(User_Login user)
        {
            var valido = await _context.Authentication.Where(x => x.User_Id == user.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            if (valido != null)
            {
                valido.Date_Expires_Token = DateTime.UtcNow.AddSeconds(-1);

                await _context.SaveChangesAsync();
            }
        }
    }
}
