using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IAppSettingRepository : IBaseRepository<AppSetting>
    {
        Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByName(string name);
    }
}
