using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToolsLibrary.Attributes.ValidationAttributes;

namespace ToolsLibrary.Models
{
    public class GeolocationAd : BaseModel
    {
        [ForeignKey(nameof(AdvertisingId))]
        public Advertisement Advertisement { get; set; }

        public int AdvertisingId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required(ErrorMessage = $"{nameof(ExpirationDate)} is required.")]
        [ExpDateValidation(ErrorMessage = "The selected date is not valid; it must be a date that is greater than today's date.")]
        public DateTime ExpirationDate { get; set; }

        public GeolocationAd()
        {
        }

        public GeolocationAd(GeolocationAd geolocationAd)
        {
            this.AdvertisingId = geolocationAd.AdvertisingId;

            this.CreateBy = geolocationAd.CreateBy;

            this.ID = geolocationAd.ID;

            this.Longitude = geolocationAd.Longitude;

            this.Latitude = geolocationAd.Latitude;
        }
    }
}