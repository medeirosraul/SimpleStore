using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleStore.Core;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Identity;
using System;
using System.Reflection;

namespace SimpleStore.Web
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
            // Database
            services.AddDbContext<StoreDbContext>(options => {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //options.LogTo(Log.Debug, Microsoft.Extensions.Logging.LogLevel.Debug);
            });

            // Identity
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<StoreIdentity>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<StoreDbContext>();

            services.AddSimpleStore("");

            // Web
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();

            // Tests
            var types = Assembly.GetEntryAssembly().GetTypes();

            Console.WriteLine("");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Use simplestore middleware
            app.UseSimpleStore();

            // Configure routes
            app.UseEndpoints(endpoints =>
            {
                // Main SimpleStore site route
                endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}")
                .RequireHost(Configuration["Host"], $"www.{Configuration["Host"]}");

                //// Admin route for Store
                //endpoints.MapAreaControllerRoute(
                //    name: "default_admin",
                //    areaName: "Admin",
                //    pattern: "Admin/{controller=Admin}/{action=Index}/{id?}"
                //    )
                //.RequireHost($"*.{Configuration["Host"]}");

                // Store route
                endpoints.MapAreaControllerRoute(
                    name: "default_store",
                    areaName: "Store",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .RequireHost($"*.{Configuration["Host"]}");
                
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToAreaPage("/Admin/{*clientroutes:nonfile}", "/_Host", "Admin")
                    .RequireHost($"*.{Configuration["Host"]}");
            });
        }
    }
}