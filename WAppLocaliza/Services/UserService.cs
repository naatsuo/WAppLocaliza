using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WAppLocaliza.Application;
using WAppLocaliza.Configuration;
using WAppLocaliza.Entities;
using WAppLocaliza.Models;

namespace WAppLocaliza.Services
{
    public class UserService : IUserService
    {
        private AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateClientUserResponse? AuthenticateClient(AuthenticateClientUserRequest model)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        var bytes = Encoding.UTF8.GetBytes(model.Password);
                        using (var hash = SHA512.Create())
                        {
                            var hashedInputBytes = hash.ComputeHash(bytes);
                            var hashedInputStringBuilder = new StringBuilder(128);
                            foreach (var b in hashedInputBytes)
                                hashedInputStringBuilder.Append(b.ToString("X2"));

                            var a = hashedInputStringBuilder.ToString();
                            var user = dbContext.ClientUsers.SingleOrDefault(i => i.Document == model.Document && i.Password == hashedInputStringBuilder.ToString());
                            if (user == null)
                                return null;

                            user.LastAccessAt = DateTime.UtcNow;
                            dbContext.SaveChanges();
                            transaction.Commit();

                            var token = GenerateJwtToken(user);
                            return new AuthenticateClientUserResponse(user, token);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public AuthenticateOperatorUserResponse? AuthenticateOperator(AuthenticateOperatorUserRequest model)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        var user = dbContext.OperatorUsers.SingleOrDefault(i => i.Number == model.Number && i.Password == model.Password);
                        if (user == null)
                            return null;

                        user.LastAccessAt = DateTime.UtcNow;
                        dbContext.SaveChanges();
                        transaction.Commit();

                        var token = GenerateJwtToken(user);
                        return new AuthenticateOperatorUserResponse(user, token);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
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

        public IEnumerable<ClientUser>? GetAll()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    return dbContext.ClientUsers.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User? GetById(Guid userId)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    return dbContext.ClientUsers.SingleOrDefault(i => i.Id == userId);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int CreateClient(CreateClientUserRequest model)
        {
            // Check Email
            try
            {
                MailAddress m = new MailAddress(model.Email);
            }
            catch (FormatException)
            {
                return -1;
            }

            // Check First Name
            if (model.FirstName.Length <= 1)
                return -2;
            // Check Last Name
            else if (model.LastName.Length <= 1)
                return -3;
            // Check Document - CPF
            else if (model.Document.Length != 14)
                return -4;
            // Check Password is too small
            else if (model.Password.Length <= 4)
                return -5;
            // Check Password is too big
            else if (model.Password.Length >= 20)
                return -6;
            else
            {
                try
                {
                    //  0 = Ok
                    // -1 = Email invalid
                    // -2 = First name invalid
                    // -3 = Last name invalid
                    // -4 = Document invalid
                    // -5 = Password is too small
                    // -6 = Password is too big
                    // -7 = Document is already being used
                    // -10 = Other
                    using (var dbContext = new ApplicationDbContext())
                    {
                        using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                        {
                            if (dbContext.ClientUsers.SingleOrDefault(i => i.Document == model.Document) is not null)
                                return -7;

                            var bytes = Encoding.UTF8.GetBytes(model.Password);
                            using (var hash = SHA512.Create())
                            {
                                var hashedInputBytes = hash.ComputeHash(bytes);
                                var hashedInputStringBuilder = new StringBuilder(128);
                                foreach (var b in hashedInputBytes)
                                    hashedInputStringBuilder.Append(b.ToString("X2"));

                                dbContext.ClientUsers.Add(new ClientUser()
                                {
                                    Document = model.Document,
                                    Password = hashedInputStringBuilder.ToString(),
                                    FirstName = model.FirstName,
                                    LastName = model.LastName,
                                    Email = model.Email,
                                    CreatedAt = DateTime.UtcNow,
                                    LastAccessAt = DateTime.UtcNow,
                                    Roles = new string[] { "Common" },
                                });

                                dbContext.SaveChanges();
                                transaction.Commit();
                                return 0;
                            }
                        }
                    }
                }
                catch
                {
                    return -10;
                }
            }
        }
    }
}