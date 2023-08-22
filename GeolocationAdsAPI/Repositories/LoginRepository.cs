using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseTool<Login>> VerifyCredential(Login credential)
        {
            ResponseTool<Login> response = null;

            try
            {
                var isValid = await _context.Logins.AnyAsync(v => v.Username == credential.Username && v.Password == credential.Password);

                if (isValid)
                {
                    response = ResponseFactory<Login>.BuildSusccess("Valid Credentail", credential, ToolsLibrary.Tools.Type.Found);
                }
                else
                {
                    response = ResponseFactory<Login>.BuildFail("Invalid Credentail", null, ToolsLibrary.Tools.Type.NotFound);
                }

                return response;
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Login>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return response;
            }
        }
    }
}