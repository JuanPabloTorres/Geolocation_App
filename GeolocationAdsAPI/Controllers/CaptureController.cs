using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ToolsLibrary.Factories;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CaptureController : ControllerBase
    {
        private readonly ICaptureRepository captureRepository;

        public CaptureController(ICaptureRepository captureRepository)
        {
            this.captureRepository = captureRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(Capture capture)
        {
            try
            {
                var _havaCaptureResponse = await this.captureRepository.CaptureExist(capture.UserId, capture.AdvertisementId);

                if (_havaCaptureResponse.ResponseType == ToolsLibrary.Tools.Type.Exist)
                {
                    return Ok(ResponseFactory<Capture>.BuildFail(_havaCaptureResponse.Message, null, _havaCaptureResponse.ResponseType));
                }

                var response = await this.captureRepository.CreateAsync(capture);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{userId}/{typeId}")]
        public async Task<IActionResult> GetMyCaptures(int userId, int typeId)
        {
            try
            {
                var response = await this.captureRepository.GetMyCaptures(userId, typeId);

                if (!response.IsSuccess)
                {
                    response = ResponseFactory<IEnumerable<Capture>>.BuildFail(response.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception));
            }
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var response = await this.captureRepository.Remove(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}