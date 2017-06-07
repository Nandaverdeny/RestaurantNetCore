using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantNetCore.Model;

namespace RestaurantNetCore.Context
{
    public class dbContext:DbContext
    {

        //5
        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }
        public DbSet<Status> Status { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Model.Type> Type { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Track> Track { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Bill> Bill { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Status>().HasKey(m => m.StatusID);
            builder.Entity<Category>().HasKey(m => m.CategoryID);
            builder.Entity<Model.Type>().HasKey(m => m.TypeID);
            builder.Entity<Model.Table>().HasKey(m => m.TableID);
            builder.Entity<Model.Menu>().HasKey(m => m.MenuID);
            builder.Entity<Order>().HasKey(m => m.OrderID);
            builder.Entity<Track>().HasKey(m => m.TrackID);
            builder.Entity<OrderItem>().HasKey(m => m.OrderItemID);
            builder.Entity<Bill>().HasKey(m => m.BillID);

            base.OnModelCreating(builder);
        }


    }
}
