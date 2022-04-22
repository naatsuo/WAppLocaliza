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
            try
            {
                var response = _userService.AuthenticateClient(model);
                if (response is not null)
                    return Ok(response);
                else
                    return new ObjectResult(new { success = false }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (UserMessageException ex)
            {
                return new ObjectResult(new { success = false, message = ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch
            {
                return new ObjectResult(new { success = false, message = "Internal Server Error" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateClientUserRequest model)
        {
            try
            {
                _userService.CreateClient(model);
                return Ok(new { success = true });
            }
            catch (UserMessageException ex)
            {
                return new ObjectResult(new { success = false, message = ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch
            {
                return new ObjectResult(new { success = false, message = "Internal Server Error" }) { StatusCode = StatusCodes.Status500InternalServerError };
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

        [AuthorizeClientUser]
        [HttpPost("SimulateCar")]
        public IActionResult SimulateCar(SimulateCarRequest model)
        {
            try
            {
                var response = _userService.SimulateCar(model);
                if (response != null)
                    return Ok(response);
                else
                    return new ObjectResult(new { success = false }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (UserMessageException ex)
            {
                return new ObjectResult(new { success = false, message = ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch
            {
                return new ObjectResult(new { success = false, message = "Internal Server Error" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        [AuthorizeClientUser]
        [HttpPost("ScheduleCar")]
        public IActionResult ScheduleCar(ScheduleCarRequest model)
        {
            try
            {
                var response = _userService.ScheduleCar(model);
                if (response != null)
                    return Ok(response);
                else
                    return new ObjectResult(new { success = false }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (UserMessageException ex)
            {
                return new ObjectResult(new { success = false, message = ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch
            {
                return new ObjectResult(new { success = false, message = "Internal Server Error" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}