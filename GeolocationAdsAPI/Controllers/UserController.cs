using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(User user)
        {
            try
            {
                user.Login.CreateDate = DateTime.Now;

                var userAddResponse = await this.userRepository.CreateAsync(user);

                if (userAddResponse.IsSuccess)
                {
                    return Ok(userAddResponse);
                }
                else
                {
                    return Ok(userAddResponse);
                }
            }
            catch (Exception ex)
            {
                var _exceptionResponse = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(_exceptionResponse);
            }
        }
    }
}