using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Market.Data.Entities.ProductAggregate;
using Market.Data.Entities.OrderAggregate;
using Market.Data.Entities.ReservationAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.EF
{
    public class MarketContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservedItem> ReservedItems { get; set; }
        public MarketContext()
        {
            Database.EnsureCreated();
        }

        public MarketContext(DbContextOptions<MarketContext> options) : base(options)
        {

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Order>().HasMany(x => x.OrderItems).WithOne(p => p.Order);
            modelBuilder.Entity<Order>().HasOne(x => x.OrderDetails).WithOne(p => p.Order).HasForeignKey<OrderDetails>(k => k.OrderId);
            modelBuilder.Entity<Reservation>().HasMany(x => x.ReservedItems).WithOne(p => p.Reservation);
            modelBuilder.Entity<Product>(ProductConfigure);

           
        }

        public void ProductConfigure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(new Product[]
           {
            new Product
            {
                Id = 1,
                Name = "TEETH PROBLEMS",
                Category = "T-SHIRT",
                Info = "100% Cotton\nIf u want one. Add in description \"I want one pls\"",
                QuantityInStock = 20,
                Price = 65
            },
            new Product {
                Id = 2,
                Name = "HEAD PROBLEMS",
                Category = "T-SHIRT",
                Info = "100% Cotton\nthoughts in my head",
                QuantityInStock = 20,
                Price = 65
            },
            new Product
            {
                Id = 3,
                Name = "THE BJORK",
                Category = "KNIT SWEATER",
                Info = "2 SIDED SWEATER WITH FULL JACQUARD\nSWEATER MADE FROM EXPENSIVE MERINOS YARN\nOVERSIZED BAGGY FIT\nWarm.Good for winter",
                QuantityInStock = 5,
                Price = 145
            }
           });
        }


    }
}
