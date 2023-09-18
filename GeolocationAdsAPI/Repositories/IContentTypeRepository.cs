using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IContentTypeRepository : IBaseRepository<ContentType>
    {
        Task<ResponseTool<IEnumerable<ContentType>>> GetContentsOfAdById(int id);

        Task<ResponseTool<bool>> CreateRangeAsync(IEnumerable<ContentType> contentTypes);

        Task<ResponseTool<bool>> RemoveAllContentOfAdvertisement(int id);
    }
}