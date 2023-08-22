using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface ILoginService : IBaseService<ToolsLibrary.Models.Login>
    {
        Task<ResponseTool<ToolsLibrary.Models.Login>> VerifyCredential(ToolsLibrary.Models.Login credential);
    }
}
