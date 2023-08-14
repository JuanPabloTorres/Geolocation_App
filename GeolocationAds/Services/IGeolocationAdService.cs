using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IGeolocationAdService : IBaseService<GeolocationAd>
    {
        Task<ResponseTool<IEnumerable<Advertisement>>> FindAdNear(CurrentLocation currentLocation);
    }
}
