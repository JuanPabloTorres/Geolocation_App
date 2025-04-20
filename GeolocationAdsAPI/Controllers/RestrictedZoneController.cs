using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestrictedZoneController : ControllerBase
    {
        private readonly IRestrictedZoneRepository restrictedZoneRepository;

        public RestrictedZoneController(IRestrictedZoneRepository restrictedZoneRepository)
        {
            this.restrictedZoneRepository = restrictedZoneRepository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllActiveRestrictedZones()
        {
            try
            {
                var response = await restrictedZoneRepository.GetAllActiveRestrictedZonesAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ResponseFactory<IEnumerable<RestrictedZone>>.BuildFail(
                    ex.Message,
                    null,
                    ToolsLibrary.Tools.Type.Exception);

                return Ok(errorResponse);
            }
        }
    }
}
