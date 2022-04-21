using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
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
                        dbContext.Operators.Add(new Operator()
                        {
                            Number = "999.999.999.99",
                            Password = "daef4953b9783365cad6615223720506cc46c5167cd16ab500fa597aa08ff964eb24fb19687f34d7665f778fcb6c5358fc0a5b81e1662cf90f73a2671c53f991",
                            FirstName = "Administrator",
                            LastName = "Bagaceira",
                            CreatedAt = DateTime.Now,
                            LastAccessAt = DateTime.Now,
                            Roles = new string[] { "admin" },
                        });

                        dbContext.SaveChanges();
                        transaction.Commit();
                    }

                    return Ok(new { message = "Success" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error" });
            }
#else
            return BadRequest(new { message = "This action only in debug mode" });
#endif
        }

    }
}