using GeolocationAdsAPI.Context;
using ToolsLibrary.Models;

namespace GeolocationAdsAPI.Repositories
{
    public class AdvertisementSettingsRepository : BaseRepositoryImplementation<AdvertisementSettings>, IAdvertisementSettingsRepository
    {
        public AdvertisementSettingsRepository(GeolocationContext context) : base(context)
        {
        }
    }
}
