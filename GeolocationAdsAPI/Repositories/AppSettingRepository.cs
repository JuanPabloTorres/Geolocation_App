using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using ToolsLibrary.Extensions;
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

        public async Task<ResponseTool<int>> GetRadiusValueByLabelAsync(string label)
        {
            try
            {
                string settingKey = $"SearchRadiusRange|{label}";

                var setting = await _context.Settings
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.SettingName == settingKey);

                if (setting.IsObjectNull())
                    return ResponseFactory<int>.BuildFail("Radius range not found.", 0);

                if (!int.TryParse(setting.Value, out int radiusValue))
                    return ResponseFactory<int>.BuildFail("Invalid numeric value for radius.", 0);

                return ResponseFactory<int>.BuildSuccess("Radius value retrieved.", radiusValue);
            }
            catch (Exception ex)
            {
                return ResponseFactory<int>.BuildFail($"An error occurred: {ex.Message}", 0);
            }
        }
    }
}