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

        public async Task<ResponseTool<bool>> AdvertisementExistInGeolocationAd(int id)
        {
            try
            {
                var _exist = await _context.GeolocationAds.AnyAsync(v => v.AdvertisingId == id);

                if (_exist)
                {

                    return ResponseFactory<bool>.BuildSusccess("Exist.", true, ToolsLibrary.Tools.Type.Exist);
                }
                else
                {
                    return ResponseFactory<bool>.BuildSusccess("Exist.", false, ToolsLibrary.Tools.Type.NotExist);
                }


            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsync()
        {
            try
            {
                var allEntities = await _context.GeolocationAds.Include(v => v.Advertisement).Where(v => DateTime.Now <= v.Advertisement.ExpirationDate).ToListAsync();

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSusccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> RemoveAllOfAdvertisementId(int id)
        {
            try
            {
                var allEntities = await _context.GeolocationAds.Where(v => v.AdvertisingId == id).ToListAsync();

                foreach (var item in allEntities)
                {
                    _context.GeolocationAds.Remove(item);
                }

                await _context.SaveChangesAsync();

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSusccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}