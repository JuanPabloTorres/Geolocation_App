using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface ICaptureRepository : IBaseRepository<Capture>
    {
        Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId);
    }
}
