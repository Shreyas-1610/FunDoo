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

        public string Login(LoginModel model);

        public bool CheckEmail(string email);

        public string GenerateToken(string Email, int userId);

        public ForgetPassModel ForgetPassword(string Email);

        public bool ResetPassword(string email, ResetPassModel model);
    }
}
