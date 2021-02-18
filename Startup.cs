using DivineMonad.Data;
using DivineMonad.Engine;
using DivineMonad.Job;
using DivineMonad.Models;
using DivineMonad.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace DivineMonad
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            services.AddSingleton<QuartzJobRunner>();
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddScoped<MarketJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(MarketJob),
                cronExpression: "0 0/5 * * * ?"));


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
