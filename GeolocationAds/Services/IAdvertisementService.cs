using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IAdvertisementService : IBaseService<Advertisement>
    {
        Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId, int? pageIndex);

        Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUserStreamedAsync(int userId, int typeId, int? pageIndex);

        Task<byte[]> GetContentVideoAsync(int id, string range);

        Task<ResponseTool<string>> GetStreamingVideoUrl(int id);
    }
}