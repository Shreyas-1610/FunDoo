using CommonLayer.Models;
using ManagerLayer.Interface;
using Repository.Context;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Service
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository user;

        public UserManager(IUserRepository user)
        {
            this.user = user;
        }

        public string Login(LoginModel model)
        {
            return user.Login(model);
        }

        public Users Registration(RegisterModel model)
        {
            return user.Registration(model);
        }

        public bool CheckEmail(string email)
        {
            return user.CheckEmail(email);
        }

        public string GenerateToken(string Email, int userId)
        {
            return user.GenerateToken(Email, userId);
        }

        public ForgetPassModel ForgetPassword(string Email)
        {
            return user.ForgetPassword(Email);
        }

        public bool ResetPassword(string email, ResetPassModel model)
        {
            return user.ResetPassword(email, model);
        }

    }
}
