using LifeQuest.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options) : base(Options) { }

        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<UserChallenge> UserChallenges { get; set; } = null!;
        public DbSet<Challenge> Challenges { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Badges> Badges { get; set; } = null!;
        public DbSet<DailyLog> DailyLogs { get; set; } = null!;
        public DbSet<MetricsCalc> Metrics { get; set; } = null!;
        public DbSet<Decision> Decisions { get; set; } = null!;
        public DbSet<Level> Levels { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId);

            modelBuilder.Entity<UserChallenge>()
                .HasKey(uc => new { uc.UserId, uc.ChallengeId });

            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.Level)
                .WithOne(l => l.UserProfile)
                .HasForeignKey<UserProfile>(up => up.LevelId);

            modelBuilder.Entity<Challenge>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<Badges>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<DailyLog>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<Decision>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<DailyLog>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<Level>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<MetricsCalc>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<UserChallenge>().HasQueryFilter(X => !X.IsDeleted);
            modelBuilder.Entity<UserProfile>().HasQueryFilter(X => !X.IsDeleted);





            base.OnModelCreating(modelBuilder);
        }
    }
}

