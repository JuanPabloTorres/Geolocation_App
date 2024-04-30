using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class CaptureRepository : BaseRepositoryImplementation<Capture>, ICaptureRepository
    {
        public CaptureRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<bool>> CaptureExist(int userId, int advertisingId)
        {
            try
            {
                var result = await _context.Captures.AnyAsync(v => v.UserId == userId && v.AdvertisementId == advertisingId);

                if (result)
                {
                    return ResponseFactory<bool>.BuildSuccess("You've already captured it.", true, ToolsLibrary.Tools.Type.Exist);
                }
                else
                {
                    return ResponseFactory<bool>.BuildFail("You haven't captured it.", false, ToolsLibrary.Tools.Type.NotExist);
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId, int typeId)
        {
            try
            {
                var result = await _context.Captures
                    .Include(a => a.Advertisements).ThenInclude(c => c.Contents)
                    .Include(s => s.Advertisements.Settings)
                    .Include(s => s.Advertisements.GeolocationAds)
                    .Where(v =>
                    v.UserId == userId &&
                    v.Advertisements.GeolocationAds.Any(g => DateTime.Now <= g.ExpirationDate) &&
                    v.Advertisements.Settings.Any(s => s.SettingId == typeId))
                    .Select(s =>
                    new Capture()
                    {
                        ID = s.ID,
                        Advertisements = new Advertisement()
                        {
                            ID = s.Advertisements.ID,
                            Contents = s.Advertisements.Contents,
                            Title = s.Advertisements.Title,
                            Description = s.Advertisements.Description
                        }
                    })
                   .ToListAsync();

                return ResponseFactory<IEnumerable<Capture>>.BuildSuccess("Entity found.", result);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Capture>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}