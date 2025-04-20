using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IRestrictedZoneRepository: IBaseRepository<RestrictedZone>
    {
        Task<ResponseTool<IEnumerable<RestrictedZone>>> GetAllActiveRestrictedZonesAsync();
    }
}
