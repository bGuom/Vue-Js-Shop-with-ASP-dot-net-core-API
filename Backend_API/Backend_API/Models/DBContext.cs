using Backend_API.Models.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;


namespace Backend_API.Models
{
    public class DBContext  : IdentityDbContext<User,UserRole,Guid> 
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Rating> Ratings { get; set; }

    }
}
