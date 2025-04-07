using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface ILoginService : IBaseService<ToolsLibrary.Models.Login>
    {
        //Task<ResponseTool<ToolsLibrary.Models.User>> VerifyCredential(ToolsLibrary.Models.Login credential);

        Task<ResponseTool<LogUserPerfilTool>> VerifyCredential2(ToolsLibrary.Models.Login credential);

        Task<ResponseTool<bool>> SignOutAsync(ToolsLibrary.Models.Login login);

    }
}
