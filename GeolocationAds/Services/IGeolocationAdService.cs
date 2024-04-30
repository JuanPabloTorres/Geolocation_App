using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IGeolocationAdService : IBaseService<GeolocationAd>
    {
        Task<ResponseTool<IEnumerable<GeolocationAd>>> FindAdsNearby(CurrentLocation currentLocation, string distance);

        Task<ResponseTool<IEnumerable<Advertisement>>> FindAdNear2(CurrentLocation currentLocation, string distance, int settinTypeId, int? pageIndex = 1);
    }
}