using CommonLayer.Models;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        public Users Registration(RegisterModel model);

        public Users Login(LoginModel model);
    }
}
