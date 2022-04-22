using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

                        var password = hashedInputStringBuilder.ToString();
                        var user = dbContext.ClientUsers.SingleOrDefault(i => i.Document == model.Document && i.Password == password);
                        if (user == null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Username or password is incorrect");

                        user.LastAccessAt = DateTime.Now;
                        dbContext.SaveChanges();
                        transaction.Commit();

                        var token = GenerateJwtToken(user);
                        return new AuthenticateClientUserResponse(user, token);
                    }
                }
            }
        }
        
        public AuthenticateOperatorUserResponse? AuthenticateOperator(AuthenticateOperatorUserRequest model)
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

                        var password = hashedInputStringBuilder.ToString();
                        var user = dbContext.OperatorUsers.SingleOrDefault(i => i.Number == model.Number && i.Password == password);
                        if (user == null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Username or password is incorrect");

                        user.LastAccessAt = DateTime.Now;

                        var token = GenerateJwtToken(user);
                        dbContext.SaveChanges();
                        transaction.Commit();
                        return new AuthenticateOperatorUserResponse(user, token);
                    }
                }
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(10),
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

        public User? GetUserById(Guid userId)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var clientUser = dbContext.ClientUsers.SingleOrDefault(i => i.Id == userId);
                    var operatorUser = dbContext.OperatorUsers.SingleOrDefault(i => i.Id == userId);
                    if (clientUser is not null)
                        return clientUser;
                    else if (operatorUser is not null)
                        return operatorUser;
                    else
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public User? GetOperatorById(Guid userId)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    return dbContext.OperatorUsers.SingleOrDefault(i => i.Id == userId);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void CreateClient(CreateClientUserRequest model)
        {
            if (!IsValidEmail(model.Email))
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Email invalid");
            else if (model.FirstName.Length <= 1)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "First name invalid");
            else if (model.LastName.Length <= 1)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Last name invalid");
            else if (!IsValidDocument(model.Document))
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Document invalid");
            else if (model.Password.Length <= 4)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Password is too small");
            else if (model.Password.Length >= 20)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Password is too big");
            else
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        if (dbContext.ClientUsers.SingleOrDefault(i => i.Document == model.Document) is not null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Document is already being used");

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
                                CreatedAt = DateTime.Now,
                                LastAccessAt = DateTime.Now,
                                Roles = new string[] { "Common" },
                            });

                            dbContext.SaveChanges();
                            transaction.Commit();
                        }
                    }
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool IsValidDocument(string document)
        {
            return Regex.IsMatch(document, @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$");
        }

        public void CreateBrand(CreateBrandRequest model)
        {
            if (model.Name.Length <= 1)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Name is too small");
            else
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        if (dbContext.CarBrands.SingleOrDefault(i => i.Name == model.Name) is not null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Name is already being used");

                        dbContext.CarBrands.Add(new CarBrand()
                        {
                            Name = model.Name,
                            Models = new List<CarModel>()
                        });

                        dbContext.SaveChanges();
                        transaction.Commit();
                    }
                }
            }
        }

        public void CreateModel(CreateModelRequest model)
        {
            Guid brandId = Guid.Empty;
            bool isValid = Guid.TryParse(model.BrandId.ToString(), out brandId);

            if (!isValid)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid brand");
            else if (model.Description.Length <= 1)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Description is too small");
            else
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        // Unknow brand
                        var carBrand = dbContext.CarBrands
                            .Include(i => i.Models)
                            .SingleOrDefault(i => i.Id == brandId);
                        if (carBrand is null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Unknow brand");

                        var carModel = dbContext.CarModels.SingleOrDefault(i => i.Description == model.Description);
                        if (carModel is not null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, " Description is already being used");

                        carBrand.Models.Add(new CarModel()
                        {
                            Name = model.Name,
                            Description = model.Description,
                            Cars = new List<Car>()
                        });

                        dbContext.SaveChanges();
                        transaction.Commit();
                    }
                }
            }
        }

        public void CreateCar(CreateCarRequest model)
        {
            Guid modelId = Guid.Empty;
            bool isValid = Guid.TryParse(model.ModelId.ToString(), out modelId);

            if (!isValid)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid model");
            else if (model.Plate.Length <= 4)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Plate is too small");
            else if (model.Year < 1886 || model.Year >= (DateTime.Now.Year + 2))
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Years out range");
            else if (model.PriceHour <= 0.0f)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid price hour");
            else if (model.TrunkLimit < 0)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid trunk limit");
            else if (model.PercentagePenalty <= 0 || model.PercentagePenalty >= 100)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid percentage penalty");

            using (var dbContext = new ApplicationDbContext())
            {
                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    var carModel = dbContext.CarModels
                        .Include(i => i.Cars)
                        .SingleOrDefault(i => i.Id == modelId);
                    if (carModel is null)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Unknow model");

                    var car = dbContext.Cars.SingleOrDefault(i => i.Plate == model.Plate);
                    if (car is not null)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Plate is already being used");

                    carModel.Cars.Add(new Car()
                    {
                        Plate = model.Plate,
                        Year = model.Year,
                        PriceHour = model.PriceHour,
                        Fuel = model.Fuel,
                        TrunkLimit = model.TrunkLimit,
                        CreatedAt = DateTime.Now,
                        PercentagePenalty = model.PercentagePenalty,
                        Schedules = new List<CarSchedule>(),
                        Histories = new List<CarHistory>(),
                    });

                    dbContext.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<CarBrand>? GetAllBrand()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    return dbContext.CarBrands
                        .Include(i => i.Models)
                        .ThenInclude(i => i.Cars)
                        .ToList();
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
                    return dbContext.CarModels
                        .Include(i => i.Cars)
                        .Include(i => i.Brand)
                        .ToList();
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
                    return dbContext.Cars
                        .Include(i => i.Schedules)
                        .Include(i => i.Model)
                        .ThenInclude(i => i.Brand)
                        .Include(i => i.Histories)
                        .ThenInclude(i => i.CheckList)
                        .ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SimulateCarResponse? SimulateCar(SimulateCarRequest model)
        {
            if (model.Start <= DateTime.Now)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid start date");
            else if ((model.End - model.Start).TotalHours < 12)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid end date");
            else
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var car = dbContext.Cars
                        .Include(i => i.Schedules)
                        .SingleOrDefault(i => i.Id == model.CarId);
                    if (car is null)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid car");

                    var hourTotal = (model.End - model.Start).TotalHours;
                    var available = !car.Schedules.Any(i => model.Start >= i.Start && model.Start <= i.End);

                    return new SimulateCarResponse()
                    {
                        CarId = model.CarId,
                        PriceHour = car.PriceHour,
                        PriceTotal = (float)(car.PriceHour * hourTotal),
                        HourTotal = hourTotal,
                        Available = available,
                        Start = model.Start,
                        End = model.End
                    };
                }
            }
        }

        public ScheduleCarResponse? ScheduleCar(ScheduleCarRequest model)
        {
            if (model.Start <= DateTime.Now)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid start date");
            else if ((model.End - model.Start).TotalHours < 12)
                throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid end date");
            else
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        var car = dbContext.Cars
                        .Include(i => i.Schedules)
                        .SingleOrDefault(i => i.Id == model.CarId);
                        if (car is null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid car");

                        var user = dbContext.ClientUsers.SingleOrDefault(i => i.Id == model.ClientId);
                        if (user is null)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid user");

                        var hourTotal = (model.End - model.Start).TotalHours;
                        var available = !car.Schedules.Any(i => model.Start >= i.Start && model.Start <= i.End);

                        if (!available)
                            throw new UserMessageException(StatusCodes.Status400BadRequest, "Car not available for schedule");
                        else
                        {
                            var schedule = new CarSchedule()
                            {
                                Start = model.Start,
                                End = model.End,
                                Price = (float)(car.PriceHour * hourTotal),
                                Note = model.Note,
                                Client = user
                            };

                            car.Schedules.Add(schedule);

                            dbContext.SaveChanges();
                            transaction.Commit();

                            return new ScheduleCarResponse()
                            {
                                ScheduleId = schedule.Id,
                                PriceHour = car.PriceHour,
                                PriceTotal = (float)(car.PriceHour * hourTotal),
                                HourTotal = hourTotal,
                                Start = model.Start,
                                End = model.End
                            };
                        }
                    }
                }
            }
        }

        public void WithdrawCar(WithdrawCarRequest model)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    var user = dbContext.ClientUsers.SingleOrDefault(i => i.Id == model.ClientId);
                    if (user is null)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid user");

                    var schedule = dbContext.CarSchedules.SingleOrDefault(i => i.Id == model.ScheduleId);
                    if (schedule is null)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid schedule");

                    if (model.WithdrawdAt <= DateTime.Now)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid withdraw date");

                    schedule.WithdrawdAt = model.WithdrawdAt;

                    dbContext.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public ReturnedCarResponse? ReturnedCar(ReturnedCarRequest model)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    var schedule = dbContext.CarSchedules
                        .Include(i => i.Client)
                        .Include(i => i.Car)
                        .ThenInclude(i => i.Histories)
                        .SingleOrDefault(i => i.Id == model.ScheduleId);

                    if (schedule is null)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid schedule");

                    if (model.ReturnedAt <= DateTime.Now)
                        throw new UserMessageException(StatusCodes.Status400BadRequest, "Invalid returned date");

                    schedule.ReturnedAt = model.ReturnedAt;

                    var realHours = ((DateTime)schedule.ReturnedAt - (DateTime)schedule.WithdrawdAt).TotalHours;
                    var scheduleHours = (schedule.End - schedule.Start).TotalHours;

                    var penaltyPrice = 0.0f;
                    if (!model.Clean)
                        penaltyPrice += (schedule.Car.PercentagePenalty / 100) * schedule.Price;
                    else if (!model.FuelTankFull)
                        penaltyPrice += (schedule.Car.PercentagePenalty / 100) * schedule.Price;
                    else if (!model.Dented)
                        penaltyPrice += (schedule.Car.PercentagePenalty / 100) * schedule.Price;
                    else if (!model.Scratched)
                        penaltyPrice += (schedule.Car.PercentagePenalty / 100) * schedule.Price;

                    schedule.RealPrice = (float)(schedule.Car.PriceHour * realHours);

                    var checkList = new CarCheckList()
                    {
                        Clean = model.Clean,
                        FuelTankFull = model.FuelTankFull,
                        Dented = model.Dented,
                        Scratched = model.Scratched,
                    };

                    var history = new CarHistory()
                    {
                        Start = schedule.Start,
                        End = schedule.End,
                        CheckList = checkList,
                        Price = schedule.Price,
                        PenaltyPrice = penaltyPrice,
                        Note = schedule.Note,
                        Client = schedule.Client,
                    };

                    schedule.Car.Histories.Add(history);

                    dbContext.SaveChanges();
                    transaction.Commit();

                    return new ReturnedCarResponse()
                    {
                        ScheduleId = schedule.Id,
                        HistoryId = history.Id,
                        Price = schedule.Price,
                        PricePenalty = penaltyPrice,
                        PriceTotal = schedule.Price + penaltyPrice,
                        HourTotal = scheduleHours
                    };
                }
            }
        }

    }
}