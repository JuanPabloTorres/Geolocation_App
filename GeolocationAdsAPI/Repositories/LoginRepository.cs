using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class LoginRepository : BaseRepositoryImplementation<Login>, ILoginRespository
    {
        public LoginRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<User>> VerifyCredential(Login credential)
        {
            ResponseTool<User> response = null;

            try
            {
                var _foundUser = await _context.Users.Include(u => u.Login).Where(v => v.Login.Username == credential.Username && v.Login.Password == credential.Password).FirstOrDefaultAsync();

                if (!_foundUser.IsObjectNull())
                {
                    response = ResponseFactory<User>.BuildSusccess("Valid Credentail", _foundUser, ToolsLibrary.Tools.Type.Found);
                }
                else
                {
                    response = ResponseFactory<User>.BuildFail("Invalid Credentail", null, ToolsLibrary.Tools.Type.NotFound);
                }

                return response;
            }
            catch (Exception ex)
            {
                response = ResponseFactory<User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }
    }
}