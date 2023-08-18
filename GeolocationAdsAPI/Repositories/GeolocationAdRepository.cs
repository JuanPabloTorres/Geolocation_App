using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class GeolocationAdRepository : BaseRepositoryImplementation<GeolocationAd>, IGeolocationAdRepository
    {
        public GeolocationAdRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsync()
        {
            try
            {
                var allEntities = await _context.GeolocationAds.Include(v => v.Advertisement).Where(v => v.Advertisement.ExpirationDate >= DateTime.Now).ToListAsync();

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSusccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}