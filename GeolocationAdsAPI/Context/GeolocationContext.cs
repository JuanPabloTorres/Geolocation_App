using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Context
{
    public class GeolocationContext : DbContext
    {
        public GeolocationContext(DbContextOptions<GeolocationContext> options) : base(options)
        {
        }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Capture> Captures { get; set; }

        public DbSet<ForgotPassword> ForgotPasswords { get; set; }

        public DbSet<GeolocationAd> GeolocationAds { get; set; }

        public DbSet<Login> Logins { get; set; }

        public DbSet<AppSetting> Settings { get; set; }

        public DbSet<User> Users { get; set; }
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
            //// Configure the Advertisement - GeolocationAd relationship
            //modelBuilder.Entity<Advertisement>()
            //    .HasOne(a => a.GeolocationAd)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade); // Set cascade delete behavior

            modelBuilder.Entity<User>()
                .HasOne(u => u.Login)
                .WithOne(l => l.User)
                .HasForeignKey<Login>(l => l.UserId)
                .IsRequired(true); // Optional, depending on your requirements

            //modelBuilder.Entity<GeolocationAd>()
            //    .HasOne(a => a.Advertisement)
            //    .WithOne(ad => ad.GeolocationAd)
            //    .OnDelete(DeleteBehavior.ClientSetNull); // Set cascade delete behavior

            //modelBuilder.Entity<Advertisement>()
            //.HasOne(a => a.GeolocationAd)
            //.WithOne(ad => ad.Advertisement)
            //.OnDelete(DeleteBehavior.Cascade); // Set cascade delete behavior

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppSetting>().HasData(
                new AppSetting { ID = 1, SettingName = "MeterDistance", Value = "10" },
                new AppSetting { ID = 2, SettingName = "MeterDistance", Value = "20" },
                new AppSetting { ID = 3, SettingName = "MeterDistance", Value = "30" }
            // Add more initial settings as needed
            );
        }
    }
}