using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByName(string name)
        {
            try
            {
                var _result = await _context.Settings.Where(v => v.SettingName == name).ToListAsync();

                return ResponseFactory<IEnumerable<AppSetting>>.BuildSusccess("Data Found", _result, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<AppSetting>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}