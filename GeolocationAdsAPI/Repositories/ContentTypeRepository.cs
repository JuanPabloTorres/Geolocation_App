using GeolocationAdsAPI.ApiTools;
using GeolocationAdsAPI.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ToolsLibrary.Extensions;
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

                return ResponseFactory<bool>.BuildSuccess("Created.", true, ToolsLibrary.Tools.Type.Added);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }

        //public async Task<ResponseTool<ContentType>> GetContentById(int contentId)
        //{
        //    try
        //    {
        //        var _contentResult = await _context.ContentTypes.Where(c => c.ID == contentId).Select(s => new ContentType() { Content = s.Content }).FirstOrDefaultAsync();

        //        if (_contentResult.IsObjectNull())
        //        {
        //            return ResponseFactory<ContentType>.BuildFail("Error Data Not Found", null, ToolsLibrary.Tools.Type.Exception);
        //        }

        //        return ResponseFactory<ContentType>.BuildSuccess("Entity found.", _contentResult, ToolsLibrary.Tools.Type.Found);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseFactory<ContentType>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        //    }
        //}

        //public async Task<ResponseTool<List<byte[]>>> GetContentById(int contentId)
        //{
        //    List<byte[]> parts = new List<byte[]>();

        //    int blockSize = 1024 * 1024; // 1MB

        //    using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        //    {
        //        await connection.OpenAsync();

        //        using (var command = new SqlCommand("SELECT Content FROM ContentTypes WHERE ID = @id", connection))
        //        {
        //            command.Parameters.AddWithValue("@id", contentId);

        //            using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    long dataIndex = 0; // Index in the DB field

        //                    byte[] buffer = new byte[blockSize];

        //                    long bytesRead;

        //                    while ((bytesRead = reader.GetBytes(0, dataIndex, buffer, 0, buffer.Length)) > 0)
        //                    {
        //                        byte[] actualRead = new byte[bytesRead];
        //                        Array.Copy(buffer, actualRead, bytesRead);
        //                        parts.Add(actualRead);
        //                        dataIndex += bytesRead;
        //                    }
        //                }
        //                else
        //                {
        //                    return ResponseFactory<List<byte[]>>.BuildFail("Error Data Not Found", null, ToolsLibrary.Tools.Type.Exception);
        //                }
        //            }
        //        }
        //    }

        //    return ResponseFactory<List<byte[]>>.BuildSuccess("Content loaded in parts.", parts, ToolsLibrary.Tools.Type.Found);
        //}

        public async Task<ResponseTool<List<byte[]>>> GetContentById(int contentId)
        {
            List<byte[]> parts = new List<byte[]>();

            int blockSize = 1024 * 1024; // 1MB

            byte[] buffer = new byte[blockSize];  // Reuse this buffer

            try
            {
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("SELECT Content FROM ContentTypes WHERE ID = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", contentId);

                        using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                        {
                            if (!await reader.ReadAsync())
                            {
                                return ResponseFactory<List<byte[]>>.BuildFail("Error Data Not Found", null, ToolsLibrary.Tools.Type.Exception);
                            }

                            long dataIndex = 0;  // Index in the DB field

                            long bytesRead;

                            while ((bytesRead = reader.GetBytes(0, dataIndex, buffer, 0, buffer.Length)) > 0)
                            {
                                byte[] actualRead = new byte[bytesRead];

                                Array.Copy(buffer, actualRead, bytesRead);

                                parts.Add(actualRead);

                                dataIndex += bytesRead;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<byte[]>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }

            return ResponseFactory<List<byte[]>>.BuildSuccess("Content loaded in parts.", parts, ToolsLibrary.Tools.Type.Found);
        }

        public async Task<ResponseTool<IEnumerable<ContentType>>> GetContentsOfAdById(int id)
        {
            try
            {
                var result = await _context.ContentTypes
                    .Where(v => v.AdvertisingId == id)
                    .Select(s => new ContentType() { ID = s.ID, AdvertisingId = s.AdvertisingId, Content = s.Content })
                    .ToListAsync();

                return ResponseFactory<IEnumerable<ContentType>>.BuildSuccess("Entity found.", result);
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

                return ResponseFactory<bool>.BuildSuccess("Entity found.", true);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<bool>> UpdateContentTypesOfAd(int adId, int updatedBy, IEnumerable<ContentType> newContentTypes)
        {
            try
            {
                var _oldContents = await this.GetContentsOfAdById(adId);

                foreach (var existTo_updatedItem in newContentTypes.ToList())
                {
                    existTo_updatedItem.Advertisement = null;

                    if (_oldContents.Data.Any(v => v.ID == existTo_updatedItem.ID))
                    {
                        if (!existTo_updatedItem.IsObjectNull() && existTo_updatedItem.Type == ContentVisualType.Video && Convert.ToUInt64(existTo_updatedItem.FileSize) == 0)
                        {
                            existTo_updatedItem.SetUpdateInformation(updatedBy);

                            existTo_updatedItem.AdvertisingId = adId;

                            var _contentBytes = await this.GetContentById(existTo_updatedItem.ID);

                            existTo_updatedItem.Content = ApiCommonsTools.Combine(_contentBytes.Data);
                        }
                    }
                    else
                    {
                        existTo_updatedItem.CreateDate = DateTime.Now;

                        existTo_updatedItem.CreateBy = updatedBy;

                        existTo_updatedItem.AdvertisingId = adId;
                    }
                }

                _context.ContentTypes.AddRange(newContentTypes);

                await this._context.SaveChangesAsync();

                return ResponseFactory<bool>.BuildSuccess("Entity found.", true);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}