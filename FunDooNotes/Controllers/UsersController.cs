using CommonLayer.Models;
using ManagerLayer.Interface;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Context;
using Repository.Entity;
using Repository.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager manager;
        private readonly IBus bus;
        private readonly FunDooDbContext context;

        public UsersController(IUserManager manager, IBus bus, FunDooDbContext context)
        {
            this.manager = manager;
            this.bus = bus;
            this.context = context;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterModel model)
        {
            var check = manager.CheckEmail(model.Email);
            if (check)
            {
                return BadRequest(new ResponseModel<Users> { Success = true, Message = "Email already exists" });
            }
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
            string login = manager.Login(model);
            if (login != null)
            {
                
                return Ok(new ResponseModel<string>{ Success = true, Message = "Login successful" ,Data = login});
            }
            else
            {
                return BadRequest(new ResponseModel<Users> { Success = false, Message = "Invalid Credential" });
            }
        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                ForgetPassModel forget = manager.ForgetPassword(email);
                SendEmailModel send = new SendEmailModel();
                send.Send(forget.Email, forget.Token);
                Uri uri = new Uri("rabbitmq://localhost/FunDooEmailQueue");
                var endpoint = await bus.GetSendEndpoint(uri);
                await endpoint.Send(forget);
                return Ok(new ResponseModel<string> {Success = true, Message = "Email sent successfully", Data =  forget.Token});
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success =false, Message = e.ToString() });
            }
        }
        //Review
        [HttpGet("GetAllUsers")]
        public IActionResult GetAll()
        {
            var users = context.Users.ToList();
            return Ok(new ResponseModel<List<Users>> { Success = true, Message = "Total users", Data = users });
        }

        [HttpGet("GetUser")]
        public IActionResult GetUserById(int userId)
        {
            var record = context.Users.FirstOrDefault(e=>e.UserId ==  userId);
            if (record != null)
            {
                return Ok(new ResponseModel<Users> { Success = true, Message = "User:",Data = record});
            }
            else
            {
                return BadRequest(new ResponseModel<Users> { Success = false, Message = "User:" });
            }
        }

        [HttpGet("UsersWithA")]
        public IActionResult starts()
        {
            var result = context.Users.Where(u => u.FirstName.StartsWith("A")).ToList();
            return Ok(new ResponseModel<List<Users>> { Success = true, Message = "User:", Data = result });
        }

        [HttpGet("GetCount")]
        public IActionResult getAllCount()
        {
            var total = context.Users.Count();
            return Ok(new ResponseModel<int> { Success = true, Message = "Total users:", Data=total });
        }

        [HttpGet("OrderByName")]
        public IActionResult orderByName()
        {
            var asc = context.Users.OrderBy(u => u.FirstName).ToList();
            //var des = context.Users.OrderByDescending(u => u.FirstName).ToList();

            return Ok(new ResponseModel<List<Users>>{Success = true, Message = "Ordered", Data = asc});
        }

        [HttpGet("OrderByDesc")]
        public IActionResult orderByNameDesc()
        {
            //var asc = context.Users.OrderBy(u => u.FirstName).ToList();
            var des = context.Users.OrderByDescending(u => u.FirstName).ToList();

            return Ok(new ResponseModel<List<Users>>{ Success = true, Message = "Ordered", Data = des });
        }
    }
}
