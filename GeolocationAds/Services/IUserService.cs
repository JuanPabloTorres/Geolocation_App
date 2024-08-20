using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<ResponseTool<bool>> IsEmailRegistered(string email);
    }
}
