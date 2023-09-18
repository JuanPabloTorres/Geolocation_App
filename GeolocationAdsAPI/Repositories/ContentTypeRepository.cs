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
                await _context.ContentTypes.AddRangeAsync(contentTypes);

                await this._context.SaveChangesAsync();


                return ResponseFactory<bool>.BuildSusccess("Created.", true, ToolsLibrary.Tools.Type.Added);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
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