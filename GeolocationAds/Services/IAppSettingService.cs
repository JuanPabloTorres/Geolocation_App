using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IAppSettingService : IBaseService<AppSetting>
    {
        Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByName(string name);

        Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByNames(IList<string> settinsName);
    }
}