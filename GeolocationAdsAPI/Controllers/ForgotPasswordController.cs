using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Dto;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        //[HttpGet("[action]/{email}")]
        //public async Task<IActionResult> RecoveryPassword(string email)
        //{
        //    ResponseTool<ForgotPassword> response;

        //    try
        //    {
        //        var _UserResponse = await this.userRepository.GetUserByEmail(email);

        //        if (_UserResponse.IsSuccess)
        //        {
        //            var _invalidUserOldForgotPassword = await this.forgotPasswordRepository.InvalidUserForgotPasswords(_UserResponse.Data.ID);

        //            if (!_invalidUserOldForgotPassword.IsSuccess)
        //            {
        //                response = ResponseFactory<ForgotPassword>.BuildFail(_invalidUserOldForgotPassword.Message, null, ToolsLibrary.Tools.Type.Fail);

        //                return Ok(response);
        //            }

        //            var _forgotPassword = new ForgotPassword()
        //            {
        //                Code = CommonsTool.GenerateRandomCode(5),
        //                UserId = _UserResponse.Data.ID,
        //                ExpTime = DateTime.Now.AddMinutes(3),
        //                CreateBy = _UserResponse.Data.ID,
        //                CreateDate = DateTime.Now,
        //                IsValid = true,
        //            };

        //            var _forgotPasswordResponse = await this.forgotPasswordRepository.CreateAsync(_forgotPassword);

        //            if (_forgotPasswordResponse.IsSuccess)
        //            {
        //                var _body = CommonsTool.HtmlEmailRecoveryDesign(_forgotPasswordResponse.Data.Code);

        //                var _emailRequest = new EmailRequest()
        //                {
        //                    Body = _body,

        //                    Subject = "Password Recovery",

        //                    To = _UserResponse.Data.Email
        //                };

        //                await CommonsTool.SendEmailAsync(_emailRequest, configuration);

        //                var _changeStatusResponse = await userRepository.ChangeStatus(_UserResponse.Data.ID, UserStatus.ResetPassword);

        //                if (_changeStatusResponse.IsSuccess)
        //                {
        //                }
        //                else
        //                {
        //                }

        //                response = ResponseFactory<ForgotPassword>.BuildSusccess("Email was sent.", null, ToolsLibrary.Tools.Type.Succesfully);

        //                return Ok(response);
        //            }
        //            else
        //            {
        //                response = ResponseFactory<ForgotPassword>.BuildFail(_forgotPasswordResponse.Message, null, ToolsLibrary.Tools.Type.Fail);

        //                return Ok(response);
        //            }
        //        }
        //        else
        //        {
        //            response = ResponseFactory<ForgotPassword>.BuildFail("User Not Found", null, ToolsLibrary.Tools.Type.NotFound);

        //            return Ok(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return Ok(response);
        //    }
        //}

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> RecoveryPassword(string email)
        {
            ResponseTool<ForgotPassword> response;

            try
            {
                var _UserResponse = await this.userRepository.GetUserByEmail(email);

                if (!_UserResponse.IsSuccess)
                {
                    response = ResponseFactory<ForgotPassword>.BuildFail(_UserResponse.Message, null, _UserResponse.ResponseType);

                    return Ok(response);
                }

                var _invalidUserOldForgotPassword = await this.forgotPasswordRepository.InvalidUserForgotPasswords(_UserResponse.Data.ID);

                if (!_invalidUserOldForgotPassword.IsSuccess)
                {
                    response = ResponseFactory<ForgotPassword>.BuildFail(_invalidUserOldForgotPassword.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

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

                if (!_forgotPasswordResponse.IsSuccess)
                {
                    response = ResponseFactory<ForgotPassword>.BuildFail(_forgotPasswordResponse.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

                var _body = CommonsTool.HtmlEmailRecoveryDesign(_forgotPasswordResponse.Data.Code);

                var _emailRequest = new EmailRequest()
                {
                    Body = _body,

                    Subject = "Password Recovery",

                    To = _UserResponse.Data.Email
                };

                await CommonsTool.SendEmailAsync(_emailRequest, configuration);

                var _changeStatusResponse = await userRepository.ChangeStatus(_UserResponse.Data.ID, UserStatus.ResetPassword);

                if (!_changeStatusResponse.IsSuccess)
                {
                    response = ResponseFactory<ForgotPassword>.BuildFail(_changeStatusResponse.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

                response = ResponseFactory<ForgotPassword>.BuildSuccess("Email was sent.", null, ToolsLibrary.Tools.Type.Succesfully);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{code}")]
        public async Task<IActionResult> ConfirmCode(string code)
        {
            ResponseTool<ForgotPassword> response;

            try
            {
                response = await this.forgotPasswordRepository.ConfirmCode(code);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(NewPasswordDto newPasswordDto)
        {
            ResponseTool<ForgotPassword> response;

            try
            {
                response = await this.forgotPasswordRepository.ChangePassword(newPasswordDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}