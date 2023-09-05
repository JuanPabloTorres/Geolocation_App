using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginRespository loginRespository;

        private IForgotPasswordRepository forgotPasswordRepository;

        public LoginController(ILoginRespository loginRespository, IForgotPasswordRepository forgotPasswordRepository)
        {
            this.loginRespository = loginRespository;

            this.forgotPasswordRepository = forgotPasswordRepository;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ResponseTool<Login> response;

            try
            {
                response = await this.loginRespository.Get(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Login>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> Update(Login login, int Id)
        {
            ResponseTool<Login> response;

            try
            {
                response = await this.loginRespository.UpdateAsync(Id, login);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Login>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyCredential(Login loginCredential)
        {
            try
            {
                var response = await this.loginRespository.VerifyCredential(loginCredential);

                if (response.IsSuccess)
                {
                    var _haveRecoveryPasswordResponse = await this.forgotPasswordRepository.UserHaveForgotPassword(response.Data.ID);

                    if (_haveRecoveryPasswordResponse.ResponseType == ToolsLibrary.Tools.Type.IsRecoveryPassword)
                    {
                        response.Message = _haveRecoveryPasswordResponse.Message;

                        response.ResponseType = _haveRecoveryPasswordResponse.ResponseType;

                        response.IsSuccess = false;
                    }

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