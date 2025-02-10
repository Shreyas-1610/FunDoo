using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Context;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Repository.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly FunDooDbContext context;
        private readonly IConfiguration config;
        public UserRepository(FunDooDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        public Users Registration(RegisterModel model)
        {
            if (CheckEmail(model.Email))
            {
                return null;
            }
            string encodedPass = EncodePass(model.Password);
            Users users = new Users();
            users.FirstName = model.FirstName;
            users.LastName = model.LastName;
            users.DOB = model.DOB;
            users.Gender = model.Gender;
            users.Email = model.Email;
            users.Password = encodedPass;
            context.Users.Add(users);
            context.SaveChanges();
            return users;
        }
        public string GenerateToken(string Email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",Email),
                new Claim("UserId",userId.ToString())
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public string Login(LoginModel model)
        {
            string hashedPassword = EncodePass(model.Password);
            var user = context.Users.FirstOrDefault(e => e.Email == model.Email && e.Password == hashedPassword);

            if (user == null)
            {
                return null; 
            }

            return GenerateToken(user.Email, user.UserId); 
        }


        private string EncodePass(string password)
        {
            try
            {
                byte[] encData= new byte[password.Length];
                encData = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public bool CheckEmail(string email)
        {
            var alreadyExist = context.Users.Any(c=>c.Email == email);
            return alreadyExist;
        }

        public ForgetPassModel ForgetPassword(string Email)
        {
            Users users = context.Users.ToList().Find(u => u.Email == Email);
            if (users != null)
            {
                ForgetPassModel forgetmodel = new ForgetPassModel();
                forgetmodel.UserId = users.UserId;
                forgetmodel.Email = users.Email;
                forgetmodel.Token = GenerateToken(users.Email, users.UserId);
                return forgetmodel;
            }
            else
            {
                throw new Exception("User does not exist!!");
            }

        }
    }
}
