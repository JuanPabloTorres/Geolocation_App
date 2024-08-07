using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.DBConfigurations
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            // Set table name
            builder.ToTable("Advertisements");

            // Configure primary key
            builder.HasKey(a => a.ID);

            // Configure relationships
            builder.HasMany(a => a.Contents)
                .WithOne(c => c.Advertisement)
                .HasForeignKey(c => c.AdvertisingId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust cascade behavior as needed

            builder.HasMany(a => a.GeolocationAds)
                .WithOne(g => g.Advertisement)
                .HasForeignKey(g => g.AdvertisingId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust cascade behavior as needed

            //builder.HasMany(a => a.Settings)
            //    .WithOne(s => s.)
            //    .HasForeignKey(s => s.AdvertisementId)
            // .OnDelete(DeleteBehavior.Cascade); // Adjust cascade behavior as needed

            // Configure properties
            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.UserId)
                .IsRequired();

            // Additional configurations can be added as needed
        }
    }
}