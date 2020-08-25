using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Environment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<ApplicationDbContext>(options => 
            {
                var connectionString = Configuration.GetConnectionString("ApplicationDbContext");

                if (Environment.IsDevelopment())
                {
                    options.UseSqlite(connectionString);
                }
                else
                {
                    options.UseSqlServer(connectionString);
                }
            });            
            services.AddTransient<IProductRepository, EFProdutRepository>();
            // Получает кллекцию служб, которые были зарегестрированны, и передает методу SessionCart.
            // Запросы для службы Cart будут обрабатываться путем создания объектов SessionCart,
            // Которые сериализуют сами себя как данные сеанса, когда они модифицируются.
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            // обработка http-запросов
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" });
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "Page{productPage:int}",
                    defaults: new { contrroller = "Product", action = "List", productPage = 1 });
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{category}",
                    defaults: new { controller = "Product", action = "List", productPage = 1});
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "",
                    defaults: new { controller = "Product", action = "List", productPage = 1 });
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "Products/Page{productPage}",
                    defaults: new { Controller = "Product", action = "List"});
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{controller=Product}/{action=List}/{id?}");
            });
            SeedData.EnsurePopulated(app);
        }
    }
}
