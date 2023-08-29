using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IForgotPasswordService : IBaseService<ForgotPassword>
    {
        Task<ResponseTool<ForgotPassword>> RecoveryPassword(string email);
    }
}
