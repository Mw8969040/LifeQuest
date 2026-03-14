using LifeQuest.DAL.Data;
using LifeQuest.DAL.Data.Seed;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.UOW.Implementation;
using LifeQuest.DAL.UOW.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LifeQuest.BLL.Mapping;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.BLL.Services.Implementation;

namespace LIfeQuest.PresentationLayer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Db Connection String Di
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // UOW Di 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AutoMapper Di
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Metrics Service Di
            builder.Services.AddScoped<IMetricsCalcService, MetricsCalcService>();

            // ApplicationDb Context Di
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var Scope = app.Services.CreateScope())
            {
                var ServiceProvider = Scope.ServiceProvider;

                var roleManager = ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                await RoleSeeder.SeedRolesAsync(roleManager);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }       

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
