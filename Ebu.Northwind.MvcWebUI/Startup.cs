using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ebu.Northwind.Business.Abstract;
using Ebu.Northwind.Business.Concrete;
using Ebu.Northwind.DataAccess.Abstract;
using Ebu.Northwind.DataAccess.Concrete.EntityFramework;
using Ebu.Northwind.MvcWebUI.Entities;
using Ebu.Northwind.MvcWebUI.Infrastructure;
using Ebu.Northwind.MvcWebUI.Middlewares;
using Ebu.Northwind.MvcWebUI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ebu.Northwind.MvcWebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvcCore();
            
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductDal, EfProductDal>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICategoryDal, EfCategoryDal>();
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<ICartSessionService, CartSessionService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();
            services.AddDbContext<ApplicationIdentityDbContext>
                (options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=true"));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseFileServer();
            app.UseNodeModules(env.ContentRootPath);

            
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSession();
            app.UseMvc(ConfigureRoutes);            
        }
        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Product}/{action=Index}/{id?}");
        }
    }
}
