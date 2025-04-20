using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
   public class RestrictedZoneService:BaseService<RestrictedZone>,IRestrictedZoneService
    {
        public RestrictedZoneService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        public Task<ResponseTool<IEnumerable<RestrictedZone>>> GetAllActiveRestrictedZones()
        {
            return HandleRequest<IEnumerable<RestrictedZone>>(async () =>
            {
                var url = $"{this.BaseApiUri}/{nameof(GetAllActiveRestrictedZones)}";

                return await _httpClient.GetAsync(url);
            });
        }
    }
}
