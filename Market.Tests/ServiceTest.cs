using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Data.Entities.ProductAggregate;
using Market.EF.Repository;
using Market.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Configuration;
using Xunit;
using Market.Data.Repositories;
using Market.Web.Controllers;
using Moq;
using Market.Domain.Interfaces;
using Market.Web.Models;
using Market.Web.Provider;
using Market.Data.Entities.OrderAggregate;
using Microsoft.AspNetCore.Http;

namespace Market.Tests
{
    public class ServiceTest
    {
        DbContextOptions<MarketContext> options = new DbContextOptionsBuilder<MarketContext>()
                  .UseInMemoryDatabase(databaseName: "Test")
                  .Options;

        [Fact]
        public void IfDbIsEmpty()
        {

            Seed();

            using (var ctx = new MarketContext(options))
            {
                Assert.Equal(3, ctx.Products.Count());
            }
        }
        

        [Fact]
        public void IfProductRepositoryWork()
        {

            Product product;
            int temp_quantity, def_quantity;
            Seed();
            using (var ctx = new MarketContext(options))
            {
                var productRepository = new ProductRepository(ctx);

                product = productRepository.GetProduct(1);
                Assert.NotNull(product);

                var category = productRepository.GetAllCategories();
                Assert.Equal(2, category.Count());

                def_quantity = product.QuantityInStock;
                productRepository.AddQuantity(1, 1);
            }
            using (var ctx = new MarketContext(options))
            {
                var productRepository = new ProductRepository(ctx);
                
                temp_quantity = product.QuantityInStock;
                productRepository.RemoveQuantity(1, 1);
                Assert.Equal(def_quantity + 1, temp_quantity);
                Assert.Equal(def_quantity, productRepository.GetProduct(product.Id).QuantityInStock);
            }
        }


        private void Seed()
        {

            using (var ctx = new MarketContext(options))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
                ctx.Products.AddRange(new Product
                {
                    Id = 1,
                    Name = "TEETH PROBLEMS",
                    Category = "T-SHIRT",
                    Info = "100% Cotton\nIf u want one. Add in description \"I want one pls\"",
                    QuantityInStock = 20,
                    Price = 65
                },
                new Product
                {
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
                });
            }
        }
    }
}
