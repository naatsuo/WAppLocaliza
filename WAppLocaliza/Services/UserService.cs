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
            //  0 = Ok
            // -1 = Email invalid
            // -2 = First name invalid
            // -3 = Last name invalid
            // -4 = Document invalid
            // -5 = Password is too small
            // -6 = Password is too big
            // -7 = Document is already being used
            // -100 = Other

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
                    return -100;
                }
            }
        }

        public int CreateBrand(CreateBrandRequest model)
        {
            //  0 = Ok
            // -1 = Check Name is too small
            // -2 = Name is already being used
            // -100 = Other

            // Check First Name
            if (model.Name.Length <= 1)
                return -1;
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        if (dbContext.CarBrands.SingleOrDefault(i => i.Name == model.Name) is not null)
                            return -2;

                        dbContext.CarBrands.Add(new CarBrand()
                        {
                            Name = model.Name,
                            Models = new List<CarModel>()
                        });

                        dbContext.SaveChanges();
                        transaction.Commit();
                        return 0;
                    }
                }
            }
            catch
            {
                return -100;
            }
        }

        public int CreateModel(CreateModelRequest model)
        {
            //  0 = Ok
            // -1 = Invalid brand
            // -2 = Unknow brand
            // -3 = Description is too small
            // -4 = Description is already being used
            // -100 = Other

            Guid brandId = Guid.Empty;
            bool isValid = Guid.TryParse(model.BrandId.ToString(), out brandId);

            // Invalid Brand
            if (!isValid)
                return -1;
            // Description is too small
            else if (model.Description.Length <= 1)
                return -3;
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        // Unknow brand
                        var carBrand = dbContext.CarBrands.SingleOrDefault(i => i.Id == brandId);
                        if (carBrand is null)
                            return -2;
                        // Description is already being used
                        var carModel = dbContext.CarModels.SingleOrDefault(i => i.Description == model.Description);
                        if (carModel is not null)
                            return -4;

                        carBrand.Models.Add(new CarModel()
                        {
                            Description = model.Description,
                            Cars = new List<Car>()
                        });

                        dbContext.SaveChanges();
                        transaction.Commit();
                        return 0;
                    }
                }
            }
            catch
            {
                return -100;
            }
        }

        public int CreateCar(CreateCarRequest model)
        {
            //  0 = Ok
            // -1 = Invalid model
            // -2 = Unknow model
            // -3 = Plate is too small
            // -4 = Plate is already being used
            // -5 = Years out range
            // -6 = Invalid price hour
            // -7 = Invalid fuel -
            // -8 = Invalid Trunk limit 
            // -9 = invalid Category -
            // -100 = Other

            Guid modelId = Guid.Empty;
            bool isValid = Guid.TryParse(model.ModelId.ToString(), out modelId);

            // Invalid Brand
            if (!isValid)
                return -1;
            // Len plate
            else if (model.Plate.Length <= 4)
                return -3;
            // Range year
            else if (model.Year < 1886 || model.Year >= (DateTime.Now.Year + 2))
                return -5;
            // Invalid price hour
            else if (model.PriceHour <= 0.0f)
                return -6;
            // Invalid Trunk limit 
            else if (model.TrunkLimit < 0)
                return -8;
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        //Unknow model
                        var carModel = dbContext.CarModels.SingleOrDefault(i => i.Id == modelId);
                        if (carModel is null)
                            return -2;

                        // plate is already being used
                        var car = dbContext.Cars.SingleOrDefault(i => i.Plate == model.Plate);
                        if (car is not null)
                            return -4;

                        carModel.Cars.Add(new Car()
                        {
                            Plate = model.Plate,
                            Year = model.Year,
                            PriceHour = model.PriceHour,
                            Fuel = model.Fuel,
                            TrunkLimit = model.TrunkLimit,
                            CreatedAt = DateTime.UtcNow,
                            Histories = new List<CarHistory>()
                        });

                        dbContext.SaveChanges();
                        transaction.Commit();
                        return 0;
                    }
                }
            }
            catch
            {
                return -100;
            }
        }

        public IEnumerable<CarBrand>? GetAllBrand()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    return dbContext.CarBrands.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<CarModel>? GetAllModel()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    return dbContext.CarModels.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<Car>? GetAllCar()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    return dbContext.Cars.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}