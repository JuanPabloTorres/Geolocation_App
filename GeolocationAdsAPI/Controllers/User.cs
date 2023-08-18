using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        private IUserRepository userRepository;

        public User(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
    }
}