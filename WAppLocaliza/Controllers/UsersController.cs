using Microsoft.AspNetCore.Mvc;
using WAppLocaliza.Authorization;
using WAppLocaliza.Models;
using WAppLocaliza.Services;

namespace WAppLocaliza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response);
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Test" });
        }

        [Authorize("Administrator")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}