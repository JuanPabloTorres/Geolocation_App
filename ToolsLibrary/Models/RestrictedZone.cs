using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary.Models
{
    public class RestrictedZone : BaseModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusInMeters { get; set; }
        public string ZoneName { get; set; }
        public bool IsExclusive { get; set; }
    }
}