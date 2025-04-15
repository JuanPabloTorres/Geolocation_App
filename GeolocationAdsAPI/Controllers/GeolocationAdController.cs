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

        private readonly IAppSettingRepository settingRepository;

        public GeolocationAdController(IGeolocationAdRepository geolocationAdRepository, IAdvertisementRepository advertisementRepository, IAppSettingRepository appSettingRepository)
        {
            this.geolocationAdRepository = geolocationAdRepository;

            this.advertisementRepository = advertisementRepository;

            this.settingRepository = appSettingRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(GeolocationAd newGeolocationAd)
        {
            ResponseTool<GeolocationAd> response;

            try
            {
                var _restrictedZone = await geolocationAdRepository.IsLocationRestrictedAsync(newGeolocationAd.Latitude, newGeolocationAd.Longitude);

                if (!_restrictedZone.IsSuccess)
                {
                    throw new Exception(_restrictedZone.Message);
                }

                var canAddMorePinResponse = await geolocationAdRepository.CanAddMorePinsAsync(newGeolocationAd.AdvertisingId);

                if (!canAddMorePinResponse.IsSuccess)
                {
                    throw new Exception(canAddMorePinResponse.Message);
                }

                var _currentAdToPost = newGeolocationAd.Advertisement;

                newGeolocationAd.Advertisement = null;

                response = await this.geolocationAdRepository.CreateAsync(newGeolocationAd);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<GeolocationAd>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]/{distance}/{settinTypeId}")]
        public async Task<IActionResult> FindAdsNearby(CurrentLocation currentLocation, string distance, int settinTypeId)
        {
            ResponseTool<IEnumerable<GeolocationAd>> response;

            try
            {
                if (currentLocation.IsObjectNull())
                {
                    throw new Exception("Current location must be provided.");
                }

                var _settingResponse = await settingRepository.GetRadiusValueByLabelAsync(distance);

                if (!_settingResponse.IsSuccess)
                {
                    return Ok(_settingResponse);
                }

                var geoAdResponse = await geolocationAdRepository.GetAllWithNavigationPropertyAsync(currentLocation.Latitude, currentLocation.Longitude, _settingResponse.Data, settinTypeId);

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
        public async Task<IActionResult> FindAdNear2(CurrentLocation currentLocation, string distance, int settingTypeId, int pageIndex)
        {
            try
            {
                var _settingResponse = await settingRepository.GetRadiusValueByLabelAsync(distance);

                if (!_settingResponse.IsSuccess)
                {
                    return Ok(_settingResponse);
                }

                var geoAdResponse = await this.geolocationAdRepository
                                              .GetAllWithNavigationPropertyAsyncAndSettingEqualTo2(currentLocation, _settingResponse.Data, settingTypeId, pageIndex);

                return Ok(geoAdResponse);
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