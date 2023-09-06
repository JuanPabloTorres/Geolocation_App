using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository userRepository;

        private ILoginRespository loginRespository;

        public UserController(IUserRepository userRepository, ILoginRespository loginRespository)
        {
            this.userRepository = userRepository;

            this.loginRespository = loginRespository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(User user)
        {
            try
            {
                user.Login.CreateDate = DateTime.Now;

                user.UserStatus = UserStatus.Active;

                user.Login.HashPassword = CommonsTool.HashPassword(user.Login.Password);

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

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ResponseTool<User> response;

            try
            {
                Expression<Func<User, object>>[] includes = { e => e.Login };

                response = await this.userRepository.Get(id, includes);

                if (!response.Data.Login.IsObjectNull())
                {
                    var _login = ModelFactory<Login>.Build(response.Data.Login);

                    response.Data.Login = new Login()
                    {
                        ID = _login.ID,
                        CreateDate = _login.CreateDate,
                        Username = _login.Username,
                        Password = _login.Password
                    };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> Update(User user, int Id)
        {
            ResponseTool<User> response;

            try
            {
                response = await this.userRepository.UpdateAsync(Id, user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }
    }
}