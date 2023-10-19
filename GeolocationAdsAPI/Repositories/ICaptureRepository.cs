using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface ICaptureRepository : IBaseRepository<Capture>
    {
        Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId, int typeId);

        Task<ResponseTool<bool>> CaptureExist(int userId, int advertisingId);
    }
}
