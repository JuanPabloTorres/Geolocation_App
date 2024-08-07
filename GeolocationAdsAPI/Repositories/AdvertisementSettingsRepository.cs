using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class AdvertisementSettingsRepository : BaseRepositoryImplementation<AdvertisementSettings>, IAdvertisementSettingsRepository
    {
        public AdvertisementSettingsRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<bool>> UpdateAdSettingByAdId(int adId, int updateBy, IEnumerable<AdvertisementSettings> adSettings)
        {
            try
            {
                var _oldSetting = await _context.AdvertisementSettings.Include(s => s.Setting).Where(v => v.AdvertisementId == adId).ToListAsync();

                if (_oldSetting.IsObjectNull())
                    return ResponseFactory<bool>.BuildFail("Data Not Found", false, ToolsLibrary.Tools.Type.NotFound);

                _context.AdvertisementSettings.RemoveRange(_oldSetting);

                foreach (var item in adSettings)
                {
                    item.AdvertisementId = adId;

                    item.SetUpdateInformation(updateBy);
                }

                _context.AdvertisementSettings.AddRange(adSettings);

                await _context.SaveChangesAsync();

                return ResponseFactory<bool>.BuildSuccess("Updated", true, ToolsLibrary.Tools.Type.Updated);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}