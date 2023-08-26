using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class GeolocationAd : BaseModel
    {
        [ForeignKey(nameof(AdvertisingId))]
        public Advertisement Advertisement { get; set; }

        public int AdvertisingId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

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