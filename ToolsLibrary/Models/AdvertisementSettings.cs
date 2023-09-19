using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToolsLibrary.Models
{
    public class AdvertisementSettings : BaseModel
    {
        [ForeignKey("AdvertisementId")]
        [JsonIgnore]
        public Advertisement Advertisement { get; set; }

        public int AdvertisementId { get; set; }

        [ForeignKey("SettingId")]
        public AppSetting Setting { get; set; }

        public int SettingId { get; set; }
    }
}