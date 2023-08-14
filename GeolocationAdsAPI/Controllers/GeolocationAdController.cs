using GeolocationAds.Tools;
using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationAdController : ControllerBase
    {
        private readonly IGeolocationAdRepository geolocationAdRepository;

        public GeolocationAdController(IGeolocationAdRepository geolocationAdRepository)
        {
            this.geolocationAdRepository = geolocationAdRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(GeolocationAd newGeolocationAd)
        {
            ResponseTool<GeolocationAd> response;

            try
            {
                response = await this.geolocationAdRepository.CreateAsync(newGeolocationAd);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<GeolocationAd>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> FindAdNear(CurrentLocation currentLocation)
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            IList<Advertisement> _adsNear = new List<Advertisement>();

            try
            {
                var _geoAd_Response = await this.geolocationAdRepository.GetAllWithNavigationPropertyAsync();

                if (_geoAd_Response.IsSuccess)
                {
                    foreach (var item in _geoAd_Response.Data)
                    {
                        double meterDistance = GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, item.Latitude, item.Longitude);

                        if (meterDistance <= 10)
                        {
                            _adsNear.Add(item.Advertisement);
                        }
                    }

                    if (_adsNear.Count > 0)
                    {
                        response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Ads Found.", _adsNear, ToolsLibrary.Tools.Type.DataFound);
                    }
                    else
                    {
                        response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Not Near Ad Found.", null, ToolsLibrary.Tools.Type.NotFound);
                    }
                }
                else
                {
                    response = ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess(_geoAd_Response.Message, null, ToolsLibrary.Tools.Type.Fail);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}