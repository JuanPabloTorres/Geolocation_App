using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IAdvertisementRepository : IBaseRepository<Advertisement>
    {
        Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId);
    }
}
