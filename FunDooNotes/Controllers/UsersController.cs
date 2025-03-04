﻿using CommonLayer.Models;
using ManagerLayer.Interface;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Repository.Context;
using Repository.Entity;
using Repository.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private readonly IDistributedCache distributedCache;

        public UsersController(IUserManager manager, IBus bus, FunDooDbContext context, IDistributedCache distributedCache)
        {
            this.manager = manager;
            this.bus = bus;
            this.context = context;
            this.distributedCache = distributedCache;
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

        [Authorize]
        [HttpPost]
        [Route("Reset")]
        public IActionResult ResetPassword(ResetPassModel resetPassModel)
        {
            try
            {
                if(resetPassModel.Password == resetPassModel.ConfirmPassword)
                {
                    string Email = User.FindFirstValue("Email");
                    if(manager.ResetPassword(Email, resetPassModel))
                    {
                        return Ok(new ResponseModel<bool> { Success = true, Message = "Reset password", Data = true});
                    }
                    else
                    {
                        return BadRequest(new ResponseModel<bool> { Success = false, Message = "Failed", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false , Message = "Password not matching", Data = false});
                }
            }
            catch(Exception e)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = e.ToString()});
            }
        }

        [HttpGet("RedisGetAllUsers")]
        public async Task<IActionResult> GetAllNotesUsingRedis()
        {
            var cacheKey = "UsersList";
            string SerializedUsersLst;
            var UsersList = new List<Users>();
            var RedisUsersList = await distributedCache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                SerializedUsersLst = Encoding.UTF8.GetString(RedisUsersList);
                UsersList = JsonConvert.DeserializeObject<List<Users>>(SerializedUsersLst);
            }
            else
            {
                UsersList = context.Users.ToList();
                SerializedUsersLst = JsonConvert.SerializeObject(UsersList);
                RedisUsersList = Encoding.UTF8.GetBytes(SerializedUsersLst);
                var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                await distributedCache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(UsersList);
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
