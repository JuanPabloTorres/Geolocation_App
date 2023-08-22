using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginRespository loginRespository;

        public LoginController(ILoginRespository loginRespository)
        {
            this.loginRespository = loginRespository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyCredential(Login loginCredential)
        {
            try
            {
                var response = await this.loginRespository.VerifyCredential(loginCredential);

                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return Ok(response);
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