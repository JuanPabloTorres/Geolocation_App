using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IAdvertisementService : IBaseService<Advertisement>
    {

        Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId, int pageIndex);

    }
}
