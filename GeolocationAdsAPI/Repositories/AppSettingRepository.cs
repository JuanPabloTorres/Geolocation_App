using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class AppSettingRepository : BaseRepositoryImplementation<AppSetting>, IAppSettingRepository
    {
        public AppSettingRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByName(string settingName)
        {
            try
            {
                var _result = await _context.Settings.Where(v => v.SettingName == settingName).ToListAsync();

                return ResponseFactory<IEnumerable<AppSetting>>.BuildSuccess("Data Found", _result, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<AppSetting>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByNames(IList<string> settingNames)
        {
            try
            {
                IList<AppSetting> appSettings = new List<AppSetting>();

                foreach (var item in settingNames)
                {
                    var _result = await _context.Settings.Where(v => v.SettingName == item).ToListAsync();

                    appSettings.AddRange(_result);
                }

                return ResponseFactory<IEnumerable<AppSetting>>.BuildSuccess("Data Found", appSettings, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<AppSetting>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}