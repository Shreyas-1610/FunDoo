using CommonLayer.Models;
using Repository.Context;
using Repository.Entity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly FunDooDbContext context;
        public UserRepository(FunDooDbContext context)
        {
            this.context = context;
        }

        public Users Registration(RegisterModel model)
        {
            Users users = new Users();
            users.FirstName = model.FirstName;
            users.LastName = model.LastName;
            users.DOB = model.DOB;
            users.Gender = model.Gender;
            users.Email = model.Email;
            users.Password = model.Password;
            context.Users.Add(users);
            context.SaveChanges();
            return users;
        }

        public Users Login(LoginModel model)
        {
            return context.Users.First(e=>e.Email == model.Email && e.Password == model.Password);           
        }
    }
}
