using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class AdvertisementSettings : BaseModel
    {
        public int SettingId { get; set; }

        [ForeignKey("SettingId")]
        public AppSetting Setting { get; set; }

        public int AdvertisementId { get; set; }
    }
}
