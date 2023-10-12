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
                _context.Advertisements.Add(advertisement);

                // Optionally, you can also add related entities in a similar way if needed.
                _context.ContentTypes.AddRange(advertisement.Contents);

                _context.AdvertisementSettings.AddRange(advertisement.Settings);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return ResponseFactory<Advertisement>.BuildSusccess("Created", null, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId)
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
                    .ToListAsync();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSusccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
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

                var _dataFoundResult = await _context.Advertisements.Include(s => s.Settings).Include(c => c.Contents).Where(v => v.ID == Id)
                    .Select(s => new Advertisement
                    {
                        ID = s.ID,
                        Description = s.Description,
                        Title = s.Title,
                        UserId = s.UserId,
                        Settings = s.Settings
                        .Select(s => new AdvertisementSettings()
                        {
                            ID = s.ID,
                            SettingId = s.SettingId
                        }).ToList(),
                        Contents = s.Contents
                            .Select(cs => new ContentType
                            {
                                ID = cs.ID,
                                Type = cs.Type,
                                Content = cs.Content
                            })
                            .ToList()
                    }).FirstOrDefaultAsync();

                return ResponseFactory<Advertisement>.BuildSusccess("Data Found", _dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public override async Task<ResponseTool<Advertisement>> UpdateAsync(int id, Advertisement updatedAdvertisement)
        {
            try
            {
                var existingAdvertisement = await _context.Advertisements
                    .Include(a => a.Contents) // Include the Contents collection if needed
                    .Include(a => a.Settings) // Include the Settings collection if needed
                    .FirstOrDefaultAsync(a => a.ID == id);

                if (!existingAdvertisement.IsObjectNull())
                {
                    // Update scalar properties
                    _context.Entry(existingAdvertisement).CurrentValues.SetValues(updatedAdvertisement);

                    // Update nested collections (e.g., Contents, GeolocationAds, Settings)

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

                    // Update scalar properties
                    _context.Entry(existingAdvertisement).CurrentValues.SetValues(updatedAdvertisement);

                    //UpdateCollection(existingAdvertisement.Settings, updatedAdvertisement.Settings);

                    await _context.SaveChangesAsync();

                    return ResponseFactory<Advertisement>.BuildSusccess("Advertisement updated successfully.", null, ToolsLibrary.Tools.Type.Updated);
                }

                return ResponseFactory<Advertisement>.BuildFail("Advertisement not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
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