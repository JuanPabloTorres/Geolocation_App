using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IAppSettingRepository : IBaseRepository<AppSetting>
    {
        Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByName(string settingName);

        Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByNames(IList<string> settingNames);
    }
}
