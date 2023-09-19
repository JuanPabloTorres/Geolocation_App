using ToolsLibrary.Models;

namespace GeolocationAds.Services
{
    public class CaptureService : BaseService<Capture>, ICaptureService
    {
        public static string _apiSuffix = nameof(CaptureService);

        public CaptureService()
        { }

        public CaptureService(string _apiSuffix) : base(_apiSuffix)
        {
        }

        public CaptureService(HttpClient htppClient, string _apiSuffix) : base(htppClient, _apiSuffix)
        {
        }
    }
}