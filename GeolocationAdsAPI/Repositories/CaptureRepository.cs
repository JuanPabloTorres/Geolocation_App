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

        public async Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId, int typeId, int pageIndex)
        {
            try
            {
                var result = await _context.Captures
                    .Where(v =>
                    v.UserId == userId &&
                    v.Advertisements.GeolocationAds.Any(g => DateTime.Now <= g.ExpirationDate) &&
                    v.Advertisements.Settings.Any(s => s.SettingId == typeId))
                    .Include(a => a.Advertisements).ThenInclude(c => c.Contents)
                    .Include(s => s.Advertisements.Settings)
                    .Include(s => s.Advertisements.GeolocationAds)
                    .Select(s =>
                    new Capture()
                    {
                        ID = s.ID,
                        CreateDate = s.CreateDate,
                        Advertisements = new Advertisement()
                        {
                            ID = s.Advertisements.ID,
                            Contents = s.Advertisements.Contents
                            .Select(ct => new ContentType
                            {
                                ID = ct.ID,
                                Type = ct.Type,
                                Content = ct.Type == ContentVisualType.Video ? Array.Empty<byte>() : ct.Content,// Apply byte range here
                                ContentName = ct.ContentName ?? string.Empty,
                                Url = ct.Type == ContentVisualType.URL ? ct.Url : string.Empty
                            })
                            .Take(1)
                            .ToList(),
                            Title = s.Advertisements.Title,
                            Description = s.Advertisements.Description
                        }
                    })
                    .OrderByDescending(ad => ad.CreateDate)
                    .Skip((pageIndex - 1) * ConstantsTools.PageSize)
                    .Take(ConstantsTools.PageSize)
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