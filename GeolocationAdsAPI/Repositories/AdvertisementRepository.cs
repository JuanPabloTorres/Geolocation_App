using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class AdvertisementRepository : BaseRepositoryImplementation<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId)
        {
            try
            {
                var _dataFoundResult = await _context.Advertisements.Include(c => c.Contents)
                    .Where(v => v.UserId == userId)
                    .Select(s =>
                    new Advertisement()
                    {
                        ID = s.ID,
                        Description = s.Description,
                        Title = s.Title,
                        UserId = s.UserId,
                        Contents = s.Contents.
                    Select(cs =>
                    new ContentType() { ID = cs.ID, Type = cs.Type, Content = cs.Content })
                    .ToList()
                    }).ToListAsync();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> VerifyExpiredAdvertimentOfUser(int userId)
        {
            try
            {
                var _dataFoundResult = await _context.Advertisements.Where(v => v.UserId == userId).ToListAsync();

                //foreach (var item in _dataFoundResult)
                //{
                //    if (DateTime.Now > item.ExpirationDate)
                //    {
                //        item.IsPosted = false;
                //    }
                //}

                await _context.SaveChangesAsync();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}