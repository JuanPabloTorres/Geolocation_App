using ToolsLibrary.Dto;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IForgotPasswordRepository : IBaseRepository<ForgotPassword>
    {
        Task<ResponseTool<ForgotPassword>> ChangePassword(NewPasswordDto newPasswordDto);

        Task<ResponseTool<ForgotPassword>> ConfirmCode(string code);

        Task<ResponseTool<IEnumerable<ForgotPassword>>> InvalidUserForgotPassword(int userId);

        Task<ResponseTool<ForgotPassword>> UserHaveForgotPassword(int userId);
    }
}