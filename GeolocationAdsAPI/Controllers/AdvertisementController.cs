using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementRepository advertisementRepository;

        private readonly IAdvertisementSettingsRepository advertisementSettingsRepository;

        public AdvertisementController(IAdvertisementRepository advertisementRepository, IAdvertisementSettingsRepository advertisementSettingsRepository)
        {
            this.advertisementRepository = advertisementRepository;

            this.advertisementSettingsRepository = advertisementSettingsRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(Advertisement advertisement)
        {
            ResponseTool<Advertisement> response;

            try
            {
                response = await this.advertisementRepository.CreateAsync(advertisement);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ResponseTool<Advertisement> response;

            try
            {
                Expression<Func<Advertisement, object>>[] geolocationAd = { e => e.GeolocationAds };

                Expression<Func<Advertisement, object>>[] settins = { e => e.Settings };

                Expression<Func<Advertisement, object>>[] contents = { e => e.Contents };

                geolocationAd = geolocationAd.Concat(settins).Concat(contents).ToArray();

                response = await this.advertisementRepository.Get(id, geolocationAd);

                if (!response.IsSuccess)
                {
                    response = ResponseFactory<Advertisement>.BuildFail(response.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

                response.Data.GeolocationAds = response.Data.GeolocationAds
                    .Select(g => new GeolocationAd() { ID = g.ID, Latitude = g.Latitude, Longitude = g.Longitude, ExpirationDate = g.ExpirationDate }).ToList();

                response.Data.Contents = response.Data.Contents
                 .Select(c => new ContentType() { ID = c.ID, AdvertisingId = c.AdvertisingId, Content = c.Content, Type = c.Type, }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetAdvertisementsOfUser(int userId)
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            try
            {
                var _verifyExpResponse = await this.advertisementRepository.VerifyExpiredAdvertimentOfUser(userId);

                if (!_verifyExpResponse.IsSuccess)
                {
                    response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(_verifyExpResponse.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

                response = await this.advertisementRepository.GetAdvertisementsOfUser(userId);

                if (!response.Data.IsObjectNull())
                {
                    response.Data = response.Data.OrderBy(o => o.CreateDate).Reverse();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            try
            {
                response = await this.advertisementRepository.GetAllAsync();

                if (!response.Data.IsObjectNull())
                {
                    response.Data = response.Data.OrderBy(o => o.CreateDate).Reverse();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            ResponseTool<Advertisement> response;

            try
            {
                response = await this.advertisementRepository.Remove(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> Update(Advertisement advertisement, int Id)
        {
            ResponseTool<Advertisement> response;

            try
            {
                Expression<Func<Advertisement, IEnumerable<object>>>[] related = { e => e.Settings };

                response = await this.advertisementRepository.UpdateAsync(Id, advertisement);

                if (response.IsSuccess)
                {
                    var _adSetting = advertisement.Settings.Where(v => v.GetType() == typeof(AdvertisementSettings)).FirstOrDefault();

                    if (_adSetting.ID == 0)
                    {
                        var _settingResponse = await this.advertisementSettingsRepository.CreateAsync(_adSetting);

                        if (!_settingResponse.IsSuccess)
                        {
                            response = ResponseFactory<Advertisement>.BuildFail(_settingResponse.Message, null, ToolsLibrary.Tools.Type.Exception);

                            return Ok(response);
                        }
                    }
                    else
                    {
                        var _settingResponse = await this.advertisementSettingsRepository.UpdateAsync(_adSetting.ID, _adSetting);

                        if (!_settingResponse.IsSuccess)
                        {
                            response = ResponseFactory<Advertisement>.BuildFail(_settingResponse.Message, null, ToolsLibrary.Tools.Type.Exception);

                            return Ok(response);
                        }
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}