using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToolsLibrary.Models
{
    public partial class Advertisement : BaseModel
    {
        public Advertisement()
        {
            this.Settings = new List<AdvertisementSettings>() {  };

            this.Contents = new List<ContentType>();

            this.Metadata = new();

        }

        public Advertisement(int userId, AdvertisementSettings settings)
        {
            this.UserId = userId;

            this.Title = string.Empty;

            this.Description = string.Empty;

            this.CreateDate = DateTime.Now;

            this.Settings = new List<AdvertisementSettings>() { settings };

            this.Contents = new List<ContentType>();

            this.Metadata = new();
        }

        public ICollection<ContentType> Contents { get; set; }

        public ICollection<GeolocationAd> GeolocationAds { get; set; }

        public ICollection<AdvertisementSettings> Settings { get; set; }

        [ObservableProperty]
        [Required(ErrorMessage = $"{nameof(Description)} is required.")]
        public string description;

        [ObservableProperty]
        [Required(ErrorMessage = $"{nameof(Title)} is required.")]
        public string title;

        public int UserId { get; set; }

        public AdvertisementMetadata Metadata { get; set; }
    }
}