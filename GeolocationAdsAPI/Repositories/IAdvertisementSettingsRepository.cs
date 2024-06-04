using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IAdvertisementSettingsRepository : IBaseRepository<AdvertisementSettings>
    {
        Task<ResponseTool<bool>> UpdateAdSettingByAdId(int adId, int updateBy, IEnumerable<AdvertisementSettings> adSettings);
    }
}
