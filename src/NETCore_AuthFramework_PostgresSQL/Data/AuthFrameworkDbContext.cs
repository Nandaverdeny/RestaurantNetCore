using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantNetCore.Models;

namespace RestaurantNetCore.Data
{
    public class AuthFrameworkDbContext : DbContext
    {
        public AuthFrameworkDbContext(DbContextOptions<AuthFrameworkDbContext> options)
            : base(options)
        {

        }

        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<ApplicationRole>().HasKey(m => m.Id);

            builder.Entity<ApplicationUserRole>().HasKey(m => m.UserId);
            builder.Entity<ApplicationUserRole>().HasKey(m => m.RoleId);


            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
