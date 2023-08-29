using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private IForgotPasswordRepository forgotPasswordRepository;

        private IUserRepository userRepository;

        private IConfiguration configuration;

        public ForgotPasswordController(IForgotPasswordRepository forgotPasswordRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            this.forgotPasswordRepository = forgotPasswordRepository;

            this.userRepository = userRepository;

            this.configuration = configuration;
        }

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> RecoveryPassword(string email)
        {
            ResponseTool<ForgotPassword> response;

            try
            {
                var _UserResponse = await this.userRepository.GetUserByEmail(email);

                if (_UserResponse.IsSuccess)
                {
                    var _forgotPassword = new ForgotPassword()
                    {
                        Code = CommonsTool.GenerateRandomCode(5),
                        UserId = _UserResponse.Data.ID,
                        ExpTime = DateTime.Now.AddMinutes(3),
                        CreateBy = _UserResponse.Data.ID,
                        CreateDate = DateTime.Now,
                        IsValid = true,
                    };

                    var _forgotPasswordResponse = await this.forgotPasswordRepository.CreateAsync(_forgotPassword);

                    if (_forgotPasswordResponse.IsSuccess)
                    {
                        var _emailRequest = new EmailRequest()
                        {
                            Body = "<div>Username" + "User01" + "</div>" + "<div>Password" + "12345" + "</div>",

                            Subject = "Password Recovery",

                            To = _UserResponse.Data.Email
                        };

                        await CommonsTool.SendEmailAsync(_emailRequest, configuration);

                        response = ResponseFactory<ForgotPassword>.BuildSusccess("Email was sent.", null, ToolsLibrary.Tools.Type.Succesfully);

                        return Ok(response);
                    }
                    else
                    {
                        response = ResponseFactory<ForgotPassword>.BuildFail(_forgotPasswordResponse.Message, null, ToolsLibrary.Tools.Type.Fail);

                        return Ok(response);
                    }
                }
                else
                {
                    response = ResponseFactory<ForgotPassword>.BuildFail("User Not Found", null, ToolsLibrary.Tools.Type.NotFound);

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response = ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}