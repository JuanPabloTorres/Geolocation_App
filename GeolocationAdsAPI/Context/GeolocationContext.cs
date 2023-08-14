using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Context
{
    public class GeolocationContext : DbContext
    {

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<GeolocationAd> GeolocationAds { get; set; }

        public GeolocationContext(DbContextOptions<GeolocationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
