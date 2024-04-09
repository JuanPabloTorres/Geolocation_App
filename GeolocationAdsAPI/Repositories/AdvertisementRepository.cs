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

                // Optionally, you can also add related entities in a similar way if needed.
                await _context.ContentTypes.AddRangeAsync(advertisement.Contents);

                await _context.AdvertisementSettings.AddRangeAsync(advertisement.Settings);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return ResponseFactory<Advertisement>.BuildSusccess("Created", null, ToolsLibrary.Tools.Type.DataFound);
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
                    .Select(s => new Advertisement
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
                            })
                            .ToList()
                    })
                    .Skip((pageIndex - 1) * ConstantsTools.PageSize)
                    .Take(ConstantsTools.PageSize)
                    .AsSplitQuery()
                    .ToListAsync();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        //public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId)
        //{
        //    try
        //    {
        //        // Use AsNoTracking for read-only scenarios to improve performance
        //        var advertisements = _context.Advertisements
        //            .AsNoTracking()
        //            .Where(v => v.UserId == userId)
        //            .OrderByDescending(s => s.CreateDate); // Keep ordering by CreateDate or any other relevant property

        //        // Filter Settings in a separate query to avoid loading unnecessary data
        //        var settingIds = await _context.Settings
        //            .AsNoTracking()
        //            .Where(s => s.ID == typeId)
        //            .Select(s => s.ID)
        //            .ToListAsync();

        //        var filteredAdvertisements = await advertisements
        //            .Where(a => settingIds.Contains(a.ID))
        //            .Select(s => new Advertisement
        //            {
        //                ID = s.ID,
        //                Description = s.Description,
        //                Title = s.Title,
        //                UserId = s.UserId,
        //                Contents = s.Contents
        //                    .Select(cs => new ContentType
        //                    {
        //                        Type = cs.Type,
        //                        Content = cs.Content
        //                    })
        //                    .ToList()
        //            })
        //            .ToListAsync();

        //        return ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Data Found", filteredAdvertisements, ToolsLibrary.Tools.Type.DataFound);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        //    }
        //}

        public override async Task<ResponseTool<Advertisement>> Get(int Id)
        {

            try
            {
                var advertisement = await _context.Advertisements
                    .Include(s => s.Settings)
                    .Include(g => g.GeolocationAds)
                    .Include(c => c.Contents)
                    .Where(v => v.ID == Id)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                if (advertisement == null)
                {
                    return ResponseFactory<Advertisement>.BuildFail("Not Found", null, ToolsLibrary.Tools.Type.NotFound);
                }

                // If the Advertisement class closely matches the desired structure, you might not need to project to a new Advertisement.
                // If you need to transform the data (e.g., for a DTO), do it here.

                return ResponseFactory<Advertisement>.BuildSusccess("Data Found", advertisement, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return ResponseFactory<Advertisement>.BuildFail("An error occurred while retrieving the advertisement.", null, ToolsLibrary.Tools.Type.Exception);
            }

            //try
            //{
            //    var _exist = await this._context.Advertisements.FindAsync(Id);

            //    if (_exist.IsObjectNull())
            //    {
            //        return ResponseFactory<Advertisement>.BuildFail("Not Found", null, ToolsLibrary.Tools.Type.NotFound);
            //    }

            //    var _dataFoundResult = await _context.Advertisements.Include(s => s.Settings).Include(g => g.GeolocationAds).Include(c => c.Contents).Where(v => v.ID == Id)
            //        .Select(s => new Advertisement
            //        {
            //            ID = s.ID,
            //            Description = s.Description,
            //            Title = s.Title,
            //            UserId = s.UserId,
            //            GeolocationAds = s.GeolocationAds
            //            .Select(s =>
            //            new GeolocationAd()
            //            {
            //                ID = s.ID,
            //                ExpirationDate = s.ExpirationDate,
            //                Latitude = s.Latitude,
            //                Longitude = s.Longitude,
            //                AdvertisingId = s.AdvertisingId
            //            }).ToList(),
            //            Settings = s.Settings
            //            .Select(s => new AdvertisementSettings()
            //            {
            //                ID = s.ID,
            //                SettingId = s.SettingId
            //            }).ToList(),
            //            Contents = s.Contents
            //                .Select(cs => new ContentType
            //                {
            //                    ID = cs.ID,
            //                    Type = cs.Type,
            //                    Content = cs.Content
            //                })
            //                .ToList()
            //        })
            //        .AsSplitQuery()
            //        .FirstOrDefaultAsync();

            //    return ResponseFactory<Advertisement>.BuildSusccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            //}
            //catch (Exception ex)
            //{
            //    return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            //}
        }

        public override async Task<ResponseTool<Advertisement>> UpdateAsync(int id, Advertisement updatedAdvertisement)
        {
            //try
            //{
            //    var existingAdvertisement = await _context.Advertisements
            //        .Include(a => a.Contents) // Include the Contents collection if needed
            //        .Include(a => a.Settings) // Include the Settings collection if needed
            //        .FirstOrDefaultAsync(a => a.ID == id);

            //    if (!existingAdvertisement.IsObjectNull())
            //    {
            //        updatedAdvertisement.CreateDate = existingAdvertisement.CreateDate;

            //        // Update scalar properties
            //        _context.Entry(existingAdvertisement).CurrentValues.SetValues(updatedAdvertisement);

            //        // Update nested collections (e.g., Contents, GeolocationAds, Settings)

            //        if (!updatedAdvertisement.Contents.IsObjectNull())
            //        {
            //            UpdateCollection(existingAdvertisement.Contents, updatedAdvertisement.Contents);
            //        }
            //        else
            //        {
            //            updatedAdvertisement.Contents = existingAdvertisement.Contents;
            //        }

            //        if (!updatedAdvertisement.Settings.IsObjectNull())
            //        {
            //            //UpdateCollection(existingAdvertisement.Contents, updatedAdvertisement.Contents);

            //            UpdateCollection(existingAdvertisement.Settings, updatedAdvertisement.Settings);
            //        }
            //        else
            //        {
            //            updatedAdvertisement.Settings = existingAdvertisement.Settings;
            //        }

            //        // Update scalar properties
            //        _context.Entry(existingAdvertisement).CurrentValues.SetValues(updatedAdvertisement);

            //        //UpdateCollection(existingAdvertisement.Settings, updatedAdvertisement.Settings);

            //        await _context.SaveChangesAsync();

            //        return ResponseFactory<Advertisement>.BuildSusccess("Advertisement updated successfully.", null, ToolsLibrary.Tools.Type.Updated);
            //    }

            //    return ResponseFactory<Advertisement>.BuildFail("Advertisement not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
            //}
            //catch (Exception ex)
            //{
            //    return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            //}

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

                return ResponseFactory<Advertisement>.BuildSusccess("Advertisement updated successfully.", null, ToolsLibrary.Tools.Type.Updated);
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

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}