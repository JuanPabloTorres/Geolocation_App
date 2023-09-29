using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

using ToolsLibrary.Factories;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                var response = await this.captureRepository.CreateAsync(capture);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetMyCaptures(int userId)
        {
            try
            {
                var response = await this.captureRepository.GetMyCaptures(userId);

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
    }
}