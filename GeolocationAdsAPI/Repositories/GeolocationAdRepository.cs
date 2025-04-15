using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Extensions;
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
                    return ResponseFactory<bool>.BuildSuccess("Exist.", true, ToolsLibrary.Tools.Type.Exist);
                }
                else
                {
                    return ResponseFactory<bool>.BuildSuccess("Exist.", false, ToolsLibrary.Tools.Type.NotExist);
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> GetAllWithNavigationPropertyAsync(double latitud, double longitude, int distance, int settinTypeId)
        {
            try
            {
                var allEntities = await _context.GeolocationAds
          .AsNoTracking()
          .Include(v => v.Advertisement)
              .ThenInclude(a => a.Settings)
          .Where(v =>
              GeolocationContext.VincentyFormulaSQL2(latitud, longitude, v.Latitude, v.Longitude) <= distance &&
              DateTime.Now <= v.ExpirationDate &&
              v.Advertisement.Settings.Any(setting => setting.SettingId == settinTypeId))
          .OrderByDescending(c => c.Advertisement.CreateDate)
          .Select(s => new GeolocationAd
          {
              Advertisement = s.Advertisement,
              Latitude = s.Latitude,
              Longitude = s.Longitude
          })
          .ToListAsync();

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSuccess("Entities fetched successfully.", allEntities);
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
                var allEntities = await _context.GeolocationAds
                    .Include(v => v.Advertisement)
                    .ThenInclude(c => c.Contents)
                    .Include(s => s.Advertisement.Settings)
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

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSuccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAllWithNavigationPropertyAsyncAndSettingEqualTo2(
    CurrentLocation currentLocation, int distance, int settingId, int pageIndex)
        {
            try
            {
                var nearbyAdIds = await _context.GeolocationAds
                    .Include(a => a.Advertisement).ThenInclude(ad => ad.Contents)
                    .Include(a => a.Advertisement).ThenInclude(ad => ad.Settings)
                    .AsNoTracking()
                    .Where(geo =>
                        GeolocationContext.VincentyFormulaSQL2(currentLocation.Latitude, currentLocation.Longitude, geo.Latitude, geo.Longitude) <= distance &&
                        DateTime.Now <= geo.ExpirationDate &&
                        geo.Advertisement.Settings.Any(s => s.SettingId == settingId))
                     .OrderByDescending(ad => ad.CreateDate)
                    .Skip((pageIndex - 1) * ConstantsTools.PageSize)
                    .Take(ConstantsTools.PageSize)
                     .Select(ad => new Advertisement
                     {
                         ID = ad.Advertisement.ID,
                         Description = ad.Advertisement.Description,
                         Title = ad.Advertisement.Title,
                         UserId = ad.Advertisement.UserId,
                         CreateDate = ad.Advertisement.CreateDate,
                         Contents = ad.Advertisement.Contents.Select(content => new ContentType
                         {
                             CreateDate = content.CreateDate,
                             ID = content.ID,
                             FileSize = content.FileSize,
                             Type = content.Type,
                             Content = content.Type == ContentVisualType.Video ? Array.Empty<byte>() : content.Content,
                             Url = content.Type == ContentVisualType.URL ? content.Url : string.Empty
                         }).ToList(),
                     })
                    .ToListAsync();

                if (nearbyAdIds.IsEmpty())
                {
                    if (pageIndex > 0)
                    {
                        return ResponseFactory<IEnumerable<Advertisement>>.BuildFail("No more nearby content found.", nearbyAdIds, ToolsLibrary.Tools.Type.EmptyCollection);
                    }

                    return ResponseFactory<IEnumerable<Advertisement>>.BuildFail("No nearby content found.", nearbyAdIds, ToolsLibrary.Tools.Type.EmptyCollection);
                }

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess("Data Found.", nearbyAdIds, ToolsLibrary.Tools.Type.DataFound);
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

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSuccess("Entities fetched successfully.", allEntities);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        /// <summary>
        /// Verifica si el anuncio puede tener más pines basándose en el valor de configuración "AdMaxPinAllow".
        /// </summary>
        /// <param name="adId">
        /// ID del anuncio
        /// </param>
        /// <returns>
        /// True si puede agregar más pines, false si alcanzó el máximo.
        /// </returns>
        public async Task<ResponseTool<bool>> CanAddMorePinsAsync(int adId)
        {
            try
            {
                // Contar cuántos pines tiene actualmente el anuncio
                var currentPinCount = await _context.GeolocationAds.CountAsync(g => g.AdvertisingId == adId);

                // Obtener el valor máximo permitido desde la configuración
                var setting = await _context.Settings.AsNoTracking().FirstOrDefaultAsync(s => s.SettingName == "AdMaxPinAllow");

                if (setting.IsObjectNull() || !int.TryParse(setting.Value, out var maxAllowed))
                {
                    throw new Exception("Could not retrieve max pin setting.");
                }

                // Verificar si aún puede agregar más pines
                var canAdd = currentPinCount < maxAllowed;

                if (!canAdd)
                {
                    return ResponseFactory<bool>.BuildFail("Have  reach max pin allow.", canAdd);
                }

                return ResponseFactory<bool>.BuildSuccess("Pin check complete.", canAdd);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail($"Error: {ex.Message}", false, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}