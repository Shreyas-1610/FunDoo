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

        public Users Login(LoginModel model)
        {
            return user.Login(model);
        }

        public Users Registration(RegisterModel model)
        {
            return user.Registration(model);
        }


    }
}
