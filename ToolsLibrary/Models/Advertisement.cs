using System.ComponentModel.DataAnnotations;

namespace ToolsLibrary.Models
{
    public class Advertisement : BaseModel
    {
        public Advertisement()
        {
        }

        public ICollection<ContentType> Contents { get; set; }
        public ICollection<GeolocationAd> GeolocationAds { get; set; }
        public ICollection<AdvertisementSettings> Settings { get; set; }

        [Required(ErrorMessage = $"{nameof(Description)} is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = $"{nameof(Title)} is required.")]
        public string Title { get; set; }

        public int UserId { get; set; }
    }
}