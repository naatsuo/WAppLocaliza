using Microsoft.AspNetCore.Mvc;
using WAppLocaliza.Authorization;
using WAppLocaliza.Models;
using WAppLocaliza.Services;

namespace WAppLocaliza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientUserController : ControllerBase
    {
        private IUserService _userService;
        public ClientUserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateClientUserRequest model)
        {
            var response = _userService.AuthenticateClient(model);
            if (response == null)
                return BadRequest(new { success = false,  message = "Username or password is incorrect" });
            return Ok(response);
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateClientUserRequest model)
        {
            var response = _userService.CreateClient(model);
            if (response == 0)
                return Ok(new { success = true});
            else if (response == -1)
                return BadRequest(new { success = false, message = "Email invalid" });
            else if (response == -2)
                return BadRequest(new { success = false, message = "First name invalid" });
            else if (response == -3)
                return BadRequest(new { success = false, message = "Last name invalid" });
            else if (response == -4)
                return BadRequest(new { success = false, message = "Document invalid" });
            else if (response == -5)
                return BadRequest(new { success = false, message = "Password is too small" });
            else if (response == -6)
                return BadRequest(new { success = false, message = "Password is too big" });
            else if (response == -7)
                return BadRequest(new { success = false, message = "Document is already being used" });
            else
                return BadRequest(new { success = false, message = "Internal error" });
        }

        //[HttpGet("Test")]
        //public IActionResult Test()
        //{
        //    return Ok(new { success = true,  message = "Test" });
        //}
        
        [AuthorizeClientUser("Administrator")]
        [HttpGet("Test")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}