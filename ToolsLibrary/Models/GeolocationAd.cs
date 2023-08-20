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
    }
}