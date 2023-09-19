using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

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
    }
}