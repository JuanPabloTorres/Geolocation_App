using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToolsLibrary.Models
{
    public class ContentType : BaseModel
    {
        public byte[] Content { get; set; }

        public ContentVisualType Type { get; set; }

        public int AdvertisingId { get; set; }

        public string ContentName { get; set; }

        public string FilePath { get; set; }

        [ForeignKey(nameof(AdvertisingId))]
        [JsonIgnore]
        public Advertisement Advertisement { get; set; }
    }

    public enum ContentVisualType
    {
        Image,
        Video,
        Unknown
    }
}
