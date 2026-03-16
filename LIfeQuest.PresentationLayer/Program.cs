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
using LifeQuest;
using LIfeQuest.BLL.Services.Implementation;

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

            // BLL Services DI
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IChallengeService, ChallengeService>();
            builder.Services.AddScoped<IDailyLogService, DailyLogService>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<ILevelService, LevelService>();
            builder.Services.AddScoped<IBadgeService, BadgeService>();
            builder.Services.AddScoped<IUserBadgeService, UserBadgeService>();
            builder.Services.AddScoped<IUserChallengeService, UserChallengeService>();
            builder.Services.AddScoped<IDecisionService, DecisionService>();
            builder.Services.AddScoped<IMetricsCalcService, MetricsCalcService>();
            builder.Services.AddScoped<IApplicationUserRegisterService, ApplicationUseRegisterService>();
            builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

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
