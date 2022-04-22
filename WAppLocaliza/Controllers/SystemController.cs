using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Cryptography;
using System.Text;
using WAppLocaliza.Application;
using WAppLocaliza.Entities;

namespace WAppLocaliza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        [HttpPost("DatabaseReset")]
        public IActionResult DatabaseReset()
        {
#if DEBUG
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    dbContext.DatabaseReset();
                    dbContext.SaveChanges();
                }
                using (var dbContext = new ApplicationDbContext())
                {
                    using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        var bytes = Encoding.UTF8.GetBytes("test123");
                        using (var hash = SHA512.Create())
                        {
                            var hashedInputBytes = hash.ComputeHash(bytes);
                            var hashedInputStringBuilder = new StringBuilder(128);
                            foreach (var b in hashedInputBytes)
                                hashedInputStringBuilder.Append(b.ToString("X2"));

                            dbContext.OperatorUsers.Add(new OperatorUser()
                            {
                                Number = "999.999.999.99",
                                Password = hashedInputStringBuilder.ToString(),
                                FirstName = "Administrator",
                                LastName = "Bagaceira",
                                CreatedAt = DateTime.UtcNow,
                                LastAccessAt = DateTime.UtcNow,
                                Roles = new string[] { "Administrator" },
                            });
                            dbContext.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    return Ok(new { message = "Success" });
                }
            }
            catch
            {
                return BadRequest(new { message = "Error" });
            }
#else
            return BadRequest(new { message = "This action only in debug mode" });
#endif
        }

    }
}