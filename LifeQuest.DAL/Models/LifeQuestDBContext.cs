using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Models
{
    public class LifeQuestDBContext :IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
    {
        public LifeQuestDBContext(DbContextOptions<LifeQuestDBContext> options) :base(options) { }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserChallenge> UserChallanges { get; set; }
        public DbSet<Challenge> Challanges { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Badges> Badges { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }
        public DbSet<MetricsCalc> Metrics { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<Level> Level { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasOne(u => u.UserProfile).WithOne(up => up.User).HasForeignKey<UserProfile>(Up => Up.UserId);
            modelBuilder.Entity<UserChallenge>().HasKey(Uc => new { Uc.UserId, Uc.ChallangeId });
            modelBuilder.Entity<UserProfile>()
    .HasOne(up => up.Level)
    .WithOne(l => l.UserProfile)
    .HasForeignKey<UserProfile>(up => up.LevelId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
