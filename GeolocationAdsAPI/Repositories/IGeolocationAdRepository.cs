using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IGeolocationAdRepository : IBaseRepository<GeolocationAd>
    {
        Task<ResponseTool<bool>> AdvertisementExistInGeolocationAd(int id);

        Task<ResponseTool<bool>> CanAddMorePinsAsync(int adId);

        Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsync(double latitud, double longitude, int distance, int settinTypeId);

        Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsyncAndSettingEqualTo(int settingId);

        Task<ResponseTool<IEnumerable<Advertisement>>> GetAllWithNavigationPropertyAsyncAndSettingEqualTo2(CurrentLocation currentLocation, int distance, int settingId, int pageIndex);

        Task<ResponseTool<bool>> IsLocationRestrictedAsync(double lat, double lng);

        Task<ResponseTool<IEnumerable<GeolocationAd>>> RemoveAllOfAdvertisementId(int id);
    }
}