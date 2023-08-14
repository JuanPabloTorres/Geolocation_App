using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IGeolocationAdRepository : IBaseRepository<GeolocationAd>
    {
        Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsync();
    }
}
