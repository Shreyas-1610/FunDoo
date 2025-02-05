using Microsoft.EntityFrameworkCore;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Context
{
    public class FunDooDbContext: DbContext
    {
        public FunDooDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Users> Users { get; set; }
    }
}
