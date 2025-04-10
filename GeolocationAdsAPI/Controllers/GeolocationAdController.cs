using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GeolocationAdController : ControllerBase
    {
        private readonly IAdvertisementRepository advertisementRepository;

        private readonly IGeolocationAdRepository geolocationAdRepository;

        public GeolocationAdController(IGeolocationAdRepository geolocationAdRepository, IAdvertisementRepository advertisementRepository)
        {
            this.geolocationAdRepository = geolocationAdRepository;

            this.advertisementRepository = advertisementRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(GeolocationAd newGeolocationAd)
        {
            ResponseTool<GeolocationAd> response;

            try
            {
                var _currentAdToPost = newGeolocationAd.Advertisement;

                newGeolocationAd.Advertisement = null;

                response = await this.geolocationAdRepository.CreateAsync(newGeolocationAd);

                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response = ResponseFactory<GeolocationAd>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]/{distance}/{settinTypeId}")]
        public async Task<IActionResult> FindAdsNearby(CurrentLocation currentLocation, int distance, int settinTypeId)
        {
            ResponseTool<IEnumerable<GeolocationAd>> response;

            if (currentLocation.IsObjectNull())
            {
                return Ok(ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail("Current location must be provided.", null, ToolsLibrary.Tools.Type.Fail));
            }

            try
            {
                var geoAdResponse = await geolocationAdRepository.GetAllWithNavigationPropertyAsync(currentLocation.Latitude, currentLocation.Longitude, distance, settinTypeId);

                if (!geoAdResponse.IsSuccess)
                {
                    return Ok(ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(geoAdResponse.Message, null, ToolsLibrary.Tools.Type.Fail));
                }

                if (!geoAdResponse.Data.Any())
                {
                    return Ok(ResponseFactory<IEnumerable<GeolocationAd>>.BuildSuccess("No nearby content.", null, ToolsLibrary.Tools.Type.NotFound));
                }

                return Ok(ResponseFactory<IEnumerable<GeolocationAd>>.BuildSuccess("Content found.", geoAdResponse.Data, ToolsLibrary.Tools.Type.DataFound));
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]/{distance}/{settingTypeId}/{pageIndex}")]
        public async Task<IActionResult> FindAdNear2(CurrentLocation currentLocation, int distance, int settingTypeId, int pageIndex)
        {
            try
            {
                var geoAdResponse = await this.geolocationAdRepository.GetAllWithNavigationPropertyAsyncAndSettingEqualTo2(currentLocation, distance, settingTypeId, pageIndex);

                if (!geoAdResponse.IsSuccess)
                {
                    return Ok(ResponseFactory<IEnumerable<Advertisement>>.BuildFail(geoAdResponse.Message, null, ToolsLibrary.Tools.Type.Fail));
                }

                var advertisements = geoAdResponse.Data;

                if (advertisements.IsObjectNull() || !advertisements.Any())
                {
                    return Ok(ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess("No nearby content found.", advertisements, ToolsLibrary.Tools.Type.NotFound));
                }

                // Assuming the data is ordered in the repository method itself
                return Ok(ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess("Content Found.", advertisements, ToolsLibrary.Tools.Type.DataFound));
            }
            catch (Exception ex)
            {
                // Consider logging the exception details here
                return Ok(ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception));
            }
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            ResponseTool<GeolocationAd> response;

            try
            {
                response = await this.geolocationAdRepository.Remove(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<GeolocationAd>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}