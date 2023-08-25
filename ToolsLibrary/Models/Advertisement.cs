using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class Advertisement : BaseModel
    {
        [Required(ErrorMessage = $"{nameof(Content)} is required.")]
        public byte[] Content { get; set; }

        [Required(ErrorMessage = $"{nameof(Description)} is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = $"{nameof(ExpirationDate)} is required.")]
        public DateTime ExpirationDate { get; set; }

        public bool IsPosted { get; set; }

        [Required(ErrorMessage = $"{nameof(Title)} is required.")]
        public string Title { get; set; }

        public int UserId { get; set; }

        public int? GeolocationAdId { get; set; }

        [ForeignKey("GeolocationAdId")]
        public GeolocationAd GeolocationAd { get; set; }

        public Advertisement()
        {
            this.ExpirationDate = DateTime.Now.AddDays(7);
        }
    }
}