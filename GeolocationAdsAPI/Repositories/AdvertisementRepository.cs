using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Extensions;
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

        public override async Task<ResponseTool<Advertisement>> CreateAsync(Advertisement advertisement)
        {
            try
            {
                // Add the advertisement entity to the DbSet
                await _context.Advertisements.AddAsync(advertisement);

                //// Optionally, you can also add related entities in a similar way if needed.
                //await _context.ContentTypes.AddRangeAsync(advertisement.Contents);

                //await _context.AdvertisementSettings.AddRangeAsync(advertisement.Settings);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return ResponseFactory<Advertisement>.BuildSuccess("Created", null, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public override async Task<ResponseTool<Advertisement>> Get(int Id)
        {
            try
            {
                var _exist = await this._context.Advertisements.FindAsync(Id);

                if (_exist.IsObjectNull())
                {
                    return ResponseFactory<Advertisement>.BuildFail("Not Found", null, ToolsLibrary.Tools.Type.NotFound);
                }

                var _dataFoundResult = await _context.Advertisements.Include(s => s.Settings).Include(g => g.GeolocationAds).Include(c => c.Contents).Where(v => v.ID == Id)
                    .Select(s => new Advertisement
                    {
                        ID = s.ID,
                        Description = s.Description,
                        Title = s.Title,
                        UserId = s.UserId,
                        GeolocationAds = s.GeolocationAds
                        .Select(s =>
                        new GeolocationAd()
                        {
                            ID = s.ID,
                            ExpirationDate = s.ExpirationDate,
                            Latitude = s.Latitude,
                            Longitude = s.Longitude,
                            AdvertisingId = s.AdvertisingId
                        }).ToList(),
                        Settings = s.Settings
                        .Select(s => new AdvertisementSettings()
                        {
                            ID = s.ID,
                            SettingId = s.SettingId
                        }).ToList(),
                        Contents = s.Contents
                            .Select(ct => new ContentType
                            {
                                ID = ct.ID,
                                Type = ct.Type,
                                Content = ct.Type == ContentVisualType.Video ? Array.Empty<byte>() : ct.Content,
                                Url = ct.Type == ContentVisualType.URL ? ct.Url : string.Empty,
                            })
                            .ToList()
                    })
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                return ResponseFactory<Advertisement>.BuildSuccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId, int pageIndex)
        {
            try
            {
                var _dataFoundResult = await _context.Advertisements.Include(s => s.Settings)
                    .Where(v =>
                    v.UserId == userId &&
                    v.Settings.Any(s => s.SettingId == typeId))
                    .OrderByDescending(s => s.CreateDate) // Order by ID (or another suitable property) if needed
                    .AsSplitQuery()
                    .Select(s => new Advertisement
                    {
                        ID = s.ID,
                        Description = s.Description,
                        Title = s.Title,
                        UserId = s.UserId,
                        Contents = s.Contents
                            .Select(ct => new ContentType
                            {
                                ID = ct.ID,
                                Type = ct.Type,
                                Content = ct.Type == ContentVisualType.Video ? Array.Empty<byte>() : ct.Content,// Apply byte range here
                                ContentName = ct.ContentName ?? string.Empty,
                                Url = ct.Type == ContentVisualType.URL ? ct.Url : string.Empty
                            })
                            .Take(1)
                            .ToList()
                    })
                    .Skip((pageIndex - 1) * ConstantsTools.PageSize)
                    .Take(ConstantsTools.PageSize)
                    .ToListAsync();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public override async Task<ResponseTool<Advertisement>> Remove(int id)
        {
            try
            {
                var _toRemoved = await _context.Advertisements.FindAsync(id);

                if (!_toRemoved.IsObjectNull())
                {
                    _context.Advertisements.Remove(_toRemoved);

                    await _context.SaveChangesAsync();

                    return ResponseFactory<Advertisement>.BuildSuccess("Item Removed.", null, ToolsLibrary.Tools.Type.Delete);
                }

                return ResponseFactory<Advertisement>.BuildFail("Item could not be removed.", null, ToolsLibrary.Tools.Type.NotFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<bool>> RemoveAdvertisement(int advertisementId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            // Execute the transactional logic within the retry policy
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var advertisement = await _context.Advertisements
                                                          .Include(ad => ad.Contents)
                                                          .Include(ad => ad.GeolocationAds)
                                                          .Include(ad => ad.Settings)
                                                          .FirstOrDefaultAsync(ad => ad.ID == advertisementId);

                        if (advertisement == null)
                        {
                            return ResponseFactory<bool>.BuildFail("Advertisement not found.", false, ToolsLibrary.Tools.Type.EntityNotFound);
                        }

                        // Delete related Contents
                        _context.ContentTypes.RemoveRange(advertisement.Contents);
                        await _context.SaveChangesAsync();

                        // Delete related GeolocationAds
                        _context.GeolocationAds.RemoveRange(advertisement.GeolocationAds);
                        await _context.SaveChangesAsync();

                        // Delete related Settings
                        _context.AdvertisementSettings.RemoveRange(advertisement.Settings);
                        await _context.SaveChangesAsync();

                        // Finally, remove the main Advertisement entity
                        _context.Advertisements.Remove(advertisement);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return ResponseFactory<bool>.BuildSuccess("Advertisement removed successfully.", true);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();

                        return ResponseFactory<bool>.BuildFail($"An error occurred: {ex.Message}", false, ToolsLibrary.Tools.Type.Exception);
                    }
                }
            });

            return ResponseFactory<bool>.BuildSuccess("Operation completed successfully", true);
        }

        public override async Task<ResponseTool<Advertisement>> UpdateAsync(int id, Advertisement updatedAdvertisement)
        {
            try
            {
                var existingAdvertisement = await _context.Advertisements
                    .Include(a => a.Contents)
                    .Include(a => a.Settings)
                    .FirstOrDefaultAsync(a => a.ID == id);

                if (existingAdvertisement == null)
                {
                    return ResponseFactory<Advertisement>.BuildFail("Advertisement not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
                }

                // Copy scalar properties
                existingAdvertisement.Title = updatedAdvertisement.Title;

                existingAdvertisement.Description = updatedAdvertisement.Description;

                existingAdvertisement.UpdateDate = DateTime.Now;

                existingAdvertisement.UpdateBy = updatedAdvertisement.UpdateBy;
                // Add more properties as needed

                // Update nested collections (Contents, Settings)
                if (!updatedAdvertisement.Contents.IsObjectNull())
                {
                    UpdateCollection(existingAdvertisement.Contents, updatedAdvertisement.Contents);
                }
                else
                {
                    updatedAdvertisement.Contents = existingAdvertisement.Contents;
                }

                if (!updatedAdvertisement.Settings.IsObjectNull())
                {
                    //UpdateCollection(existingAdvertisement.Contents, updatedAdvertisement.Contents);

                    UpdateCollection(existingAdvertisement.Settings, updatedAdvertisement.Settings);
                }
                else
                {
                    updatedAdvertisement.Settings = existingAdvertisement.Settings;
                }

                await _context.SaveChangesAsync();

                return ResponseFactory<Advertisement>.BuildSuccess("Advertisement updated successfully.", null, ToolsLibrary.Tools.Type.Updated);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> VerifyExpiredAdvertimentOfUser(int userId)
        {
            try
            {
                var _dataFoundResult = await _context.Advertisements.Where(v => v.UserId == userId).ToListAsync();

                await _context.SaveChangesAsync();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}