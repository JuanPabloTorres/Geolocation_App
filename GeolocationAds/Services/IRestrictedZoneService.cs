using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IRestrictedZoneService:IBaseService<RestrictedZone>
    {
        Task<ResponseTool<IEnumerable<RestrictedZone>>> GetAllActiveRestrictedZones();
    }
}
