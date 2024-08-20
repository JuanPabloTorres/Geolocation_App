using GeolocationAdsAPI.ApiTools;
using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRespository loginRespository;

        private readonly IForgotPasswordRepository forgotPasswordRepository;

        private readonly IConfiguration _config;

        public LoginController(ILoginRespository loginRespository, IForgotPasswordRepository forgotPasswordRepository, IConfiguration config)
        {
            this.loginRespository = loginRespository;

            this.forgotPasswordRepository = forgotPasswordRepository;

            _config = config;
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

        //[HttpPost("[action]")]
        //[AllowAnonymous]
        //public async Task<IActionResult> VerifyCredential(Login loginCredential)
        //{
        //    try
        //    {
        //        ResponseTool<User> response;

        //        if (loginCredential.Provider == ToolsLibrary.Enums.Providers.Google)
        //        {
        //            response = await this.loginRespository.VerifyCredentialByProvider(loginCredential);
        //        }
        //        else
        //        {
        //            response = await this.loginRespository.VerifyCredential(loginCredential);
        //        }

        //        if (response.IsSuccess)
        //        {
        //            // Authenticate the user and generate a JWT token.
        //            var token = JwtTool.GenerateJSONWebToken(this._config, response.Data.ID.ToString());

        //            return Ok(response);
        //        }
        //        else
        //        {
        //            return Ok(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _exceptionResponse = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return Ok(_exceptionResponse);
        //    }
        //}

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCredential(Login loginCredential)
        {
            try
            {
                ResponseTool<User> response;

                // Verifica el proveedor y llama al método correspondiente
                switch (loginCredential.Provider)
                {
                    case ToolsLibrary.Enums.Providers.Google:
                        response = await this.loginRespository.VerifyCredentialByProvider(loginCredential);
                        break;

                    case ToolsLibrary.Enums.Providers.Facebook:
                        response = await this.loginRespository.VerifyCredentialByProvider(loginCredential);
                        break;

                    default:
                        // Por defecto, usa la verificación estándar
                        response = await this.loginRespository.VerifyCredential(loginCredential);
                        break;
                }

                if (response.IsSuccess)
                {
                    // Autenticar al usuario y generar un token JWT
                    var token = JwtTool.GenerateJSONWebToken(this._config, response.Data.ID.ToString());

                    return Ok(token);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var exceptionResponse = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(exceptionResponse);
            }
        }


        //[HttpPost("[action]")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GenerateJSONWebToken()
        //{
        //    try
        //    {
        //        var response = await this.loginRespository.VerifyCredential(loginCredential);

        //        if (response.IsSuccess)
        //        {
        //            // Authenticate the user and generate a JWT token.
        //            var token = JwtTool.GenerateJSONWebToken(this._config, response.Data.ID.ToString());

        //            return Ok(response);
        //        }
        //        else
        //        {
        //            return Ok(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _exceptionResponse = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return Ok(_exceptionResponse);
        //    }
        //}

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCredential2(Login loginCredential)
        {
            try
            {
                //var response = await this.loginRespository.VerifyCredential(loginCredential);

                ResponseTool<User> response;

                // Verifica el proveedor y llama al método correspondiente
                switch (loginCredential.Provider)
                {
                    case ToolsLibrary.Enums.Providers.Google:
                        response = await this.loginRespository.VerifyCredentialByProvider(loginCredential);
                        break;

                    case ToolsLibrary.Enums.Providers.Facebook:
                        response = await this.loginRespository.VerifyCredentialByProvider(loginCredential);
                        break;

                    default:
                        // Por defecto, usa la verificación estándar
                        response = await this.loginRespository.VerifyCredential(loginCredential);
                        break;
                }

                if (response.IsSuccess)
                {
                    // Authenticate the user and generate a JWT token.
                    //var token = JwtTool.GenerateJwtToken(response.Data.ID.ToString());

                    var token = JwtTool.GenerateJSONWebToken(this._config, response.Data.ID.ToString());

                    var _userPerfil = new LogUserPerfilTool()
                    {
                        JsonToken = token,
                        LogUser = response.Data
                    };

                    var _perfilResponse = ResponseFactory<LogUserPerfilTool>.BuildSuccess(response.Message, _userPerfil);

                    return Ok(_perfilResponse);
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