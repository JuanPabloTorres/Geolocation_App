using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class Capture : BaseModel
    {
        public int AdvertisementId { get; set; }

        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisements { get; set; }

        public int UserId { get; set; }
    }
}
