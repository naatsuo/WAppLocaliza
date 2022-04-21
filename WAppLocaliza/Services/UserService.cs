using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WAppLocaliza.Configuration;
using WAppLocaliza.Entities;
using WAppLocaliza.Models;

namespace WAppLocaliza.Services
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            new User{Id = Guid.Parse("98D05E5D-4CE7-4CC1-A474-433B333135CA"), Document = "000.000.000-00", Password = "admin1234", Roles = new[]{ "User", "Administrator" } },
            new User{Id = Guid.Parse("3B1C0B4C-7867-4F95-99FD-C2A2F195FE1A"), Document = "000.000.000-01", Password = "user1234", Roles = new[]{ "User" } },
        };
        private AppSettings _appSettings;
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse? Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(i => i.Document == model.Document && i.Password == model.Password);
            if (user == null) return null;
            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User? GetById(Guid userId)
        {
            return _users.SingleOrDefault(i => i.Id == userId);
        }
    }
}