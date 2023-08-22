using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface ILoginRespository : IBaseRepository<Login>
    {
        Task<ResponseTool<Login>> VerifyCredential(Login credential);
    }
}