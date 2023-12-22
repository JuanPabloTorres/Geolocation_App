using GeolocationAds.Tools;
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
                var allEntities = await _context.GeolocationAds.Include(v => v.Advertisement).Where(v => DateTime.Now <= v.ExpirationDate).ToListAsync();

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSusccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsyncAndSettingEqualTo(int settingId)
        {
            try
            {
                var allEntities = await _context.GeolocationAds.Include(v => v.Advertisement).ThenInclude(c => c.Contents).Include(s => s.Advertisement.Settings)
                    .Where(v =>
                    DateTime.Now <= v.ExpirationDate &&
                    v.Advertisement.Settings.Any(s => s.SettingId == settingId))
                    .Select(s => new GeolocationAd
                    {
                        ID = s.ID,
                        ExpirationDate = s.ExpirationDate,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude,

                        Advertisement = new Advertisement
                        {
                            ID = s.AdvertisingId,
                            Description = s.Advertisement.Description,
                            Title = s.Advertisement.Title,
                            UserId = s.Advertisement.UserId,
                            Contents = s.Advertisement.Contents
                            .Select(cs => new ContentType
                            {
                                Type = cs.Type,
                                Content = cs.Content
                            })
                            .ToList()
                        }
                    })
                    .ToListAsync();

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSusccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAllWithNavigationPropertyAsyncAndSettingEqualTo2(CurrentLocation currentLocation, int distance, int settingId)
        {
            try
            {
                Func<Advertisement, bool> filterCondition = v =>
                v.GeolocationAds.Any(g =>
                DateTime.Now <= g.ExpirationDate &&
                GeolocationTool.VincentyFormula4(currentLocation.Latitude, currentLocation.Longitude, g.Latitude, g.Longitude) <= distance) &&
                v.Settings.Any(s => s.SettingId == settingId);

                var allEntities = _context.Advertisements.Include(c => c.Contents).Include(g => g.GeolocationAds).Include(s => s.Settings)
                   .Where(filterCondition)
                   .Select(s =>

                        new Advertisement
                        {
                            ID = s.ID,
                            Description = s.Description,
                            Title = s.Title,
                            UserId = s.UserId,
                            Contents = s.Contents
                           .Select(cs => new ContentType
                           {
                               Type = cs.Type,
                               Content = cs.Content
                           }).ToList(),
                        }
                   ).ToList();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
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