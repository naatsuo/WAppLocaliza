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
            var response = _userService.AuthenticateOperator(model);
            if (response == null)
                return BadRequest(new { success = false, message = "Number or password is incorrect" });
            return Ok(response);
        }

        /*
       cadastrar/buscar marca
       cadastrar/buscar modelo na marca
       cadastrar/buscar carro no modelo

       simulação de locação
       agendamento de um veiculo
       gerar um pdf com as informãções do banco

       checklist de devolução
       */

        [AuthorizeOperatorUser]
        [HttpPost("CreateBrand")]
        public IActionResult CreateBrand(CreateBrandRequest model)
        {
            var response = _userService.CreateBrand(model);
            switch (response)
            {
                case 0:
                    return Ok(new { success = true });
                case -1:
                    return BadRequest(new { success = false, message = "Name is too small" });
                case -2:
                    return BadRequest(new { success = false, message = "Name is already being used" });
                case -100:
                    return BadRequest(new { success = false, message = "Internal error" });
                default:
                    return BadRequest(new { success = false });
            }
        }

        [AuthorizeOperatorUser]
        [HttpPost("CreateModel")]
        public IActionResult CreateModel(CreateModelRequest model)
        {
            var response = _userService.CreateModel(model);
            switch (response)
            {
                case 0:
                    return Ok(new { success = true });
                case -1:
                    return BadRequest(new { success = false, message = "Invalid brand" });
                case -2:
                    return BadRequest(new { success = false, message = "Unknow brand" });
                case -3:
                    return BadRequest(new { success = false, message = "Description is too small" });
                case -4:
                    return BadRequest(new { success = false, message = "Description is already being used" });
                case -100:
                    return BadRequest(new { success = false, message = "Internal error" });
                default:
                    return BadRequest(new { success = false });
            }
        }

        [AuthorizeOperatorUser]
        [HttpPost("CreateCar")]
        public IActionResult CreateCar(CreateCarRequest model)
        {
            var response = _userService.CreateCar(model);
            switch (response)
            {
                case 0:
                    return Ok(new { success = true });
                case -1:
                    return BadRequest(new { success = false, message = "Invalid model" });
                case -2:
                    return BadRequest(new { success = false, message = "Unknow model" });
                case -3:
                    return BadRequest(new { success = false, message = "Plate is too small" });
                case -4:
                    return BadRequest(new { success = false, message = "Plate is already being used" });
                case -5:
                    return BadRequest(new { success = false, message = "Years out range" });
                case -6:
                    return BadRequest(new { success = false, message = "Invalid price hour" });
                case -7: //
                    return BadRequest(new { success = false });
                case -8:
                    return BadRequest(new { success = false, message = "Invalid Trunk limit" });
                case -9: //
                    return BadRequest(new { success = false });
                case -100:
                    return BadRequest(new { success = false, message = "Internal error" });
                default:
                    return BadRequest(new { success = false });
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



        [AuthorizeOperatorUser("Administrator")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}