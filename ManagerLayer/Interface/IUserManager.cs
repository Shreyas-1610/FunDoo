using CommonLayer.Models;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface IUserManager
    {
        public Users Registration(RegisterModel model);

        public Users Login(LoginModel model);
    }
}
