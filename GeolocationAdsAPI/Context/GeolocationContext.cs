using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Context
{
    public class GeolocationContext : DbContext
    {

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<GeolocationAd> GeolocationAds { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Login> Logins { get; set; }

        public DbSet<AppSetting> Settings { get; set; }

        public DbSet<ForgotPassword> ForgotPasswords { get; set; }

        public GeolocationContext(DbContextOptions<GeolocationContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        // Seed method to populate initial AppSetting values
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSetting>().HasData(
                new AppSetting { ID = 1, SettingName = "MeterDistance", Value = "10" },
                new AppSetting { ID = 2, SettingName = "MeterDistance", Value = "20" },
                new AppSetting { ID = 3, SettingName = "MeterDistance", Value = "30" }
            // Add more initial settings as needed
            );
        }
    }
}
