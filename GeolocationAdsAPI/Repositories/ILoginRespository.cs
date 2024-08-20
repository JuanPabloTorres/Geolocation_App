using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface ILoginRespository : IBaseRepository<Login>
    {
        Task<ResponseTool<User>> VerifyCredential(Login credential);

        Task<ResponseTool<User>> VerifyCredentialByProvider(Login credential);
    }
}