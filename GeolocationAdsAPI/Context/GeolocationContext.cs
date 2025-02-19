using GeolocationAdsAPI.DBConfigurations;
using Microsoft.Data.SqlClient;
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

        public DbSet<AdvertisementSettings> AdvertisementSettings { get; set; }

        public DbSet<Capture> Captures { get; set; }

        public DbSet<ContentType> ContentTypes { get; set; }

        public DbSet<ForgotPassword> ForgotPasswords { get; set; }

        public DbSet<GeolocationAd> GeolocationAds { get; set; }

        public DbSet<Login> Logins { get; set; }

        public DbSet<AppSetting> Settings { get; set; }

        public DbSet<User> Users { get; set; }

        [DbFunction("VincentyFormulaSQL2", "dbo")]
        public static double VincentyFormulaSQL2(double lat1, double lon1, double lat2, double lon2)
        {
            throw new NotSupportedException();
        }

        // Define a method to execute the stored procedure
        public async Task<int> RemoveAdvertisement(int advertisementId)
        {
            var advertisementIdParameter = new SqlParameter("@AdvertisementId", advertisementId);

            return await Database.ExecuteSqlRawAsync("EXEC RemoveAdvertisement @AdvertisementId", advertisementIdParameter);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        // Seed method to populate initial AppSetting values
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Login)
                .WithOne(l => l.User)
                .HasForeignKey<Login>(l => l.UserId)
                .IsRequired(true); // Optional, depending on your requirements

          

            modelBuilder.Entity<Advertisement>()
                .HasMany(a => a.Settings)
                .WithOne()
                .HasForeignKey(ad => ad.AdvertisementId);

            modelBuilder.ApplyConfiguration(new AdvertisementConfiguration());

            modelBuilder.Entity<AdvertisementSettings>()
                .HasOne(ads => ads.Setting);

            modelBuilder.Entity<AppSetting>().HasData(
                new AppSetting { ID = 1, SettingName = "MeterDistance", Value = "10" },
                new AppSetting { ID = 2, SettingName = "MeterDistance", Value = "20" },
                new AppSetting { ID = 3, SettingName = "MeterDistance", Value = "30" },
                new AppSetting { ID = 4, SettingName = "MeterDistance", Value = "40" },
                new AppSetting { ID = 5, SettingName = "MeterDistance", Value = "50" },
                new AppSetting { ID = 6, SettingName = "AdTypes", Value = "Broadcast" },
                new AppSetting { ID = 7, SettingName = "AdTypes", Value = "Social" },
                new AppSetting { ID = 8, SettingName = "AdTypes", Value = "Store" },
                new AppSetting { ID = 9, SettingName = "AdTypes", Value = "Offer" },
                new AppSetting { ID = 10, SettingName = "AdTypes", Value = "News" }
            // Add more initial settings as needed
            );
        }
    }
}