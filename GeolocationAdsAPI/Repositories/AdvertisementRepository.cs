using GeolocationAdsAPI.Context;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Repositories
{
    public class AdvertisementRepository : BaseRepositoryImplementation<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(GeolocationContext context) : base(context)
        {
        }
    }
}
