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

        public async Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId)
        {
            try
            {
                var result = await _context.Captures
                    .Include(a => a.Advertisements).ThenInclude(c => c.Contents)
                    .Where(v => v.UserId == userId)
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

                return ResponseFactory<IEnumerable<Capture>>.BuildSusccess("Entity found.", result);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Capture>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}