using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
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

        public AdvertisementController(IAdvertisementRepository advertisementRepository)
        {
            this.advertisementRepository = advertisementRepository;
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
                response = await this.advertisementRepository.Get(id);

                if (!response.IsSuccess)
                {
                    response = ResponseFactory<Advertisement>.BuildFail(response.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{userId}/{typeId}")]
        public async Task<IActionResult> GetAdvertisementsOfUser(int userId, int typeId)
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            try
            {
                response = await this.advertisementRepository.GetAdvertisementsOfUser(userId, typeId);

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
                response = await this.advertisementRepository.UpdateAsync(Id, advertisement);

                if (!response.IsSuccess)
                {
                    response = ResponseFactory<Advertisement>.BuildFail(response.Message, null, ToolsLibrary.Tools.Type.Exception);

                    return Ok(response);
                }

                //var _adSetting = advertisement.Settings.Where(v => v.GetType() == typeof(AdvertisementSettings)).FirstOrDefault();

                //var _settingResponse = await this.advertisementSettingsRepository.UpdateAsync(_adSetting.ID, _adSetting);

                //if (!_settingResponse.IsSuccess)
                //{
                //    response = ResponseFactory<Advertisement>.BuildFail(_settingResponse.Message, null, ToolsLibrary.Tools.Type.Exception);

                //    return Ok(response);
                //}

                //if (response.Data.Contents.Count() > 0)
                //{
                //    var _removeAllResponse = await this.contentTypeRepository.RemoveAllContentOfAdvertisement(Id);

                //    if (!_removeAllResponse.IsSuccess)
                //    {
                //        return Ok(ResponseFactory<Advertisement>.BuildFail(_removeAllResponse.Message, null, ToolsLibrary.Tools.Type.Exception));
                //    }

                //    await this.contentTypeRepository.CreateRangeAsync(response.Data.Contents);
                //}

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