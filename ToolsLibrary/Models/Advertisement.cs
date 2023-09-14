using System.ComponentModel.DataAnnotations;
using ToolsLibrary.Attributes.ValidationAttributes;

namespace ToolsLibrary.Models
{
    public class Advertisement : BaseModel
    {
        public Advertisement()
        {
            this.ExpirationDate = DateTime.Now.AddDays(7);
        }

        [Required(ErrorMessage = $"{nameof(Content)} is required.")]
        public byte[] Content { get; set; }

        [Required(ErrorMessage = $"{nameof(Description)} is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = $"{nameof(ExpirationDate)} is required.")]
        [ExpDateValidation(ErrorMessage = "The selected date is not valid; it must be a date that is greater than today's date.")]
        public DateTime ExpirationDate { get; set; }

        public ICollection<GeolocationAd> GeolocationAds { get; set; }

        public ICollection<ContentType> Contents { get; set; }

        public ICollection<AdvertisementSettings> Settings { get; set; }

        public bool IsPosted { get; set; }

        [Required(ErrorMessage = $"{nameof(Title)} is required.")]
        public string Title { get; set; }

        public int UserId { get; set; }
    }
}