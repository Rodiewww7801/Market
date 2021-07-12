using Market.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Data.Repositories;
using Market.EF.Repository;
using Market.Mongo;
using Microsoft.Extensions.Options;
using Market.Mongo.Repositories;
using Market.Domain.Interfaces;
using Market.Web.Provider;
using Microsoft.AspNetCore.Http;
using Market.Domain.Implementations;
using FluentValidation;
using Market.Data.Entities.OrderAggregate;
using Market.Web.Validators;
using FluentValidation.AspNetCore;
using ReservationCheckWorker;
using Microsoft.Extensions.Logging;
using Market.Domain.Logger;

namespace Market.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation();
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }
            );


            services.AddTransient<MongoContext>(option => new MongoContext(Configuration.GetConnectionString("MongoConnection")));
            services.AddDbContext<MarketContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICartProvider, CartProvider>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IValidator<OrderDetails>, OrderDetailsValidator>();

            services.AddHostedService<WorkerService>();
            services.AddScoped<ReservationChecker>();

           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            RegisterClassMap.Register();
            loggerFactory.AddFile(AppDomain.CurrentDomain.BaseDirectory + "/log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=ProductList}/{id?}");
            });
             




        }


    }
}
