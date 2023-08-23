using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface ILoginService : IBaseService<ToolsLibrary.Models.Login>
    {
        Task<ResponseTool<ToolsLibrary.Models.User>> VerifyCredential(ToolsLibrary.Models.Login credential);
    }
}
