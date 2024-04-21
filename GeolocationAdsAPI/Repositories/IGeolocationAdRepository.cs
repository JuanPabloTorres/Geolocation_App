using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IGeolocationAdRepository : IBaseRepository<GeolocationAd>
    {
        Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsync();

        Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsyncAndSettingEqualTo(int settingId);

        Task<ResponseTool<IEnumerable<Advertisement>>> GetAllWithNavigationPropertyAsyncAndSettingEqualTo2(CurrentLocation currentLocation, int distance, int settingId,int pageIndex);

        Task<ResponseTool<IEnumerable<GeolocationAd>>> RemoveAllOfAdvertisementId(int id);

        Task<ResponseTool<bool>> AdvertisementExistInGeolocationAd(int id);
    }
}