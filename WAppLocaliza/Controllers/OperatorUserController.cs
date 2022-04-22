using Microsoft.AspNetCore.Mvc;
using WAppLocaliza.Authorization;
using WAppLocaliza.Entities;
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
            try
            {
                var response = _userService.AuthenticateOperator(model);
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

        [AuthorizeOperatorUser]
        [HttpPost("CreateBrand")]
        public IActionResult CreateBrand(CreateBrandRequest model)
        {
            try
            {
                _userService.CreateBrand(model);
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

        [AuthorizeOperatorUser]
        [HttpPost("CreateModel")]
        public IActionResult CreateModel(CreateModelRequest model)
        {
            try
            {
                _userService.CreateModel(model);
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

        [AuthorizeOperatorUser]
        [HttpPost("CreateCar")]
        public IActionResult CreateCar(CreateCarRequest model)
        {
            try
            {
                _userService.CreateCar(model);
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

        [AuthorizeOperatorUser]
        [HttpPost("GetAllBrand")]
        public IActionResult GetAllBrand()
        {
            var brands = _userService.GetAllBrand();
            return Ok(brands);
        }

        [AuthorizeOperatorUser]
        [HttpPost("GetAllModel")]
        public IActionResult GetAllModel()
        {
            var models = _userService.GetAllModel();
            return Ok(models);
        }

        [AuthorizeOperatorUser]
        [HttpPost("GetAllCar")]
        public IActionResult GetAllCar()
        {
            var cars = _userService.GetAllCar();
            return Ok(cars);
        }

        [AuthorizeOperatorUser]
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

        [AuthorizeOperatorUser]
        [HttpPost("WithdrawCar")]
        public IActionResult WithdrawCar(WithdrawCarRequest model)
        {
            try
            {
                _userService.WithdrawCar(model);
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

        [AuthorizeOperatorUser]
        [HttpPost("ReturnedCar")]
        public IActionResult ReturnedCar(ReturnedCarRequest model)
        {
            try
            {
                var response = _userService.ReturnedCar(model);
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