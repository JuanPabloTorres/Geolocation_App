using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class ContentTypeRepository : BaseRepositoryImplementation<ContentType>, IContentTypeRepository
    {
        public ContentTypeRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<bool>> CreateRangeAsync(IEnumerable<ContentType> contentTypes)
        {
            try
            {
                // Using a temporary in-memory list to avoid issues with EF Core tracking
                var tempList = contentTypes.ToList();

                // Disable change tracking for the entities to improve performance
                _context.ChangeTracker.AutoDetectChangesEnabled = false;

                // Add the entities to the context without tracking them
                _context.ContentTypes.AddRange(tempList);

                // Save changes in a batch
                await _context.SaveChangesAsync();

                // Re-enable change tracking
                _context.ChangeTracker.AutoDetectChangesEnabled = true;

                return ResponseFactory<bool>.BuildSusccess("Created.", true, ToolsLibrary.Tools.Type.Added);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<ContentType>> GetContentById(int contentId)
        {
            try
            {
                var _contentResult = await _context.ContentTypes.FindAsync(contentId);

                return ResponseFactory<ContentType>.BuildSusccess("Entity found.", _contentResult, ToolsLibrary.Tools.Type.Found);
            }
            catch (Exception ex)
            {
                return ResponseFactory<ContentType>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<ContentType>>> GetContentsOfAdById(int id)
        {
            try
            {
                var result = await _context.ContentTypes
                    .Where(v => v.AdvertisingId == id)
                    .Select(s => new ContentType() { ID = s.ID, AdvertisingId = s.AdvertisingId, Content = s.Content })
                    .ToListAsync();

                return ResponseFactory<IEnumerable<ContentType>>.BuildSusccess("Entity found.", result);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<ContentType>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<bool>> RemoveAllContentOfAdvertisement(int id)
        {
            try
            {
                var result = await _context.ContentTypes
                    .Where(v => v.AdvertisingId == id)
                    .ToListAsync();

                _context.ContentTypes.RemoveRange(result);

                await this._context.SaveChangesAsync();

                return ResponseFactory<bool>.BuildSusccess("Entity found.", true);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}