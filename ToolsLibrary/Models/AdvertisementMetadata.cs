using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToolsLibrary.Models
{
    public partial class AdvertisementMetadata:BaseModel
    {
        [ForeignKey("AdvertisementId")]
        [JsonIgnore]
        public Advertisement Advertisement { get; set; }

        public int AdvertisementId { get; set; }

        // 🔗 Enlaces externos
        public string? FacebookUrl { get; set; }

        public string? InstagramUrl { get; set; }

        public string? WebsiteUrl { get; set; }

        // 📍 Localización opcional adicional (por ejemplo, una tienda o evento)
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        // 🗓️ Fecha de evento (si aplica)
        public DateTime? EventDate { get; set; }

        // 🔒 Número máximo de veces que puede ser capturado
        public int? MaxCaptured { get; set; }  // null significa sin límite

        // 📞 Información de contacto
        public string? ContactEmail { get; set; }

        public string? ContactPhone { get; set; }
    }
}
