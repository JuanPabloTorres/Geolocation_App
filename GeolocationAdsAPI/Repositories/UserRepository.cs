using GeolocationAdsAPI.Context;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class UserRepository : BaseRepositoryImplementation<User>, IUserRepository
    {
        public UserRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<Login>> AddLoginCredential(Login credential)
        {
            ResponseTool<Login> response;

            try
            {
                credential.CreateDate = DateTime.Now;

                await _context.AddAsync(credential);

                await _context.SaveChangesAsync();

                response = ResponseFactory<Login>.BuildSusccess("Entity created successfully.", credential, ToolsLibrary.Tools.Type.Added);

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