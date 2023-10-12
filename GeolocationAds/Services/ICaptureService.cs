using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface ICaptureService : IBaseService<Capture>
    {
        Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId, int typeId);
    }
}