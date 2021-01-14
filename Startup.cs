using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using DivineMonad.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DivineMonad.Models;
using DivineMonad.Engine;
using DivineMonad.Tools;
using Quartz.Spi;
using DivineMonad.Job;
using Quartz;
using Quartz.Impl;

namespace DivineMonad
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
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.User.RequireUniqueEmail = true;
                    })
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IItemRepo, ItemRepo>();
            services.AddScoped<IItemStatsRepo, ItemStatsRepo>();
            services.AddScoped<ICharacterItemsRepo, CharacterItemsRepo>();
            services.AddScoped<ICharacterBaseStatsRepo, CharacterBaseStatsRepo>();
            services.AddScoped<IRarityRepo, RarityRepo>();
            services.AddScoped<IAdvanceStats, AdvanceStats>();
            services.AddScoped<IFightGenerator, FightGenerator>();
            services.AddScoped<ICharacterHelper, CharacterHelper>();
            services.AddScoped<IDbContextHelper, DbContextHelper>();

            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<MarketJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(MarketJob),
                cronExpression: "0/20 * * * * ?"));


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("user",
                    builder => builder.RequireRole("Admin", "User"));
                options.AddPolicy("admin",
                    builder => builder.RequireRole("Admin"));
            });

            services.AddHostedService<QuartzHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
