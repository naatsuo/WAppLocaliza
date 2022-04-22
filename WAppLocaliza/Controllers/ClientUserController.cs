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
                return BadRequest(new { success = false, message = "Username or password is incorrect" });
            return Ok(response);
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateClientUserRequest model)
        {
            var response = _userService.CreateClient(model);
            switch (response)
            {
                case 0:
                    return Ok(new { success = true });
                case -1:
                    return BadRequest(new { success = false, message = "Email invalid" });
                case -2:
                    return BadRequest(new { success = false, message = "First name invalid" });
                case -3:
                    return BadRequest(new { success = false, message = "Last name invalid" });
                case -4:
                    return BadRequest(new { success = false, message = "Document invalid" });
                case -5:
                    return BadRequest(new { success = false, message = "Password is too small" });
                case -6:
                    return BadRequest(new { success = false, message = "Password is too big" });
                case -7:
                    return BadRequest(new { success = false, message = "Document is already being used" });
                case -100:
                    return BadRequest(new { success = false, message = "Internal error" });
                default:
                    return BadRequest(new { success = false });
            }
        }

        [AuthorizeClientUser]
        [HttpPost("GetAllBrand")]
        public IActionResult GetAllBrand()
        {
            var brands = _userService.GetAllBrand();
            return Ok(brands);
        }

        [AuthorizeClientUser]
        [HttpPost("GetAllModel")]
        public IActionResult GetAllModel()
        {
            var models = _userService.GetAllModel();
            return Ok(models);
        }

        [AuthorizeClientUser]
        [HttpPost("GetAllCar")]
        public IActionResult GetAllCar()
        {
            var cars = _userService.GetAllCar();
            return Ok(cars);
        }

    }
}