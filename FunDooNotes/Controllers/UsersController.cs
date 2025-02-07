using CommonLayer.Models;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Entity;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager manager;

        public UsersController(IUserManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterModel model)
        {
            var result = manager.Registration(model);
            if (result != null)
            {
                return Ok(new ResponseModel<Users> { Success = true , Message = "Successfully registered", Data = result});
            }
            else
            {
                return BadRequest(new ResponseModel<Users> { Success = false, Message = "Registration failed"});
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel model)
        {
            var login = manager.Login(model);
            if (login != null)
            {
                return Ok(new ResponseModel<Users> { Success = true, Message = "Login successful", Data=login });
            }
            else
            {
                return BadRequest(new ResponseModel<Users> { Success = false, Message = "Invalid Credential" });
            }
        }
    }
}
