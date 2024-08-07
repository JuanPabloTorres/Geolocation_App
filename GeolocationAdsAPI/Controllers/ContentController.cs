using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentTypeRepository _contentTypeRepository;

        public ContentController(IContentTypeRepository contentTypeRepository)
        {
                _contentTypeRepository = contentTypeRepository;
        }


    }
}
