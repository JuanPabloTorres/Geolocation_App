using GeolocationAdsAPI.Context;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Repositories
{
    public class CaptureRepository : BaseRepositoryImplementation<Capture>, ICaptureRepository
    {
        public CaptureRepository(GeolocationContext context) : base(context)
        {
        }
    }
}
