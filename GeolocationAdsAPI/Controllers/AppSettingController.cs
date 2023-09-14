using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingController : ControllerBase
    {
        private IAppSettingRepository appSettingRepository;

        public AppSettingController(IAppSettingRepository appSettingRepository)
        {
            this.appSettingRepository = appSettingRepository;
        }

        [HttpGet("[action]/{settingName}")]
        public async Task<IActionResult> GetAppSettingByName(string settingName)
        {
            ResponseTool<IEnumerable<AppSetting>> response;

            try
            {
                response = await this.appSettingRepository.GetAppSettingByName(settingName);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<AppSetting>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetAppSettingByNames(IList<string> settingNames)
        {
            ResponseTool<IEnumerable<AppSetting>> response;

            try
            {
                response = await this.appSettingRepository.GetAppSettingByNames(settingNames);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<AppSetting>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}