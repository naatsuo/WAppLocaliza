using Microsoft.AspNetCore.Mvc;
using WAppLocaliza.Authorization;
using WAppLocaliza.Models;
using WAppLocaliza.Services;

namespace WAppLocaliza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperatorUserController : ControllerBase
    {
        private IUserService _userService;
        public OperatorUserController(IUserService userService)
        {
            _userService = userService;
        }
      
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateOperatorUserRequest model)
        {
            var response = _userService.AuthenticateOperator(model);
            if (response == null)
                return BadRequest(new { message = "Number or password is incorrect" });
            return Ok(response);
        }


        [AuthorizeOperatorUser("Administrator")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}