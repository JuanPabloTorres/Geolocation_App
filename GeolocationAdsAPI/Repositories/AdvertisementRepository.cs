using GeolocationAdsAPI.ApiTools;
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
        private readonly IContentTypeRepository contentTypeRepository;

        private readonly IAdvertisementSettingsRepository advertisementSettingsRepository;

        public AdvertisementRepository(GeolocationContext context, IContentTypeRepository contentTypeRepository, IAdvertisementSettingsRepository advertisementSettingsRepository) : base(context)
        {
            this.contentTypeRepository = contentTypeRepository;

            this.advertisementSettingsRepository = advertisementSettingsRepository;
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

                var _dataFoundResult = await _context.Advertisements.Where(v => v.ID == Id).Include(s => s.Settings).Include(g => g.GeolocationAds).Include(c => c.Contents)
                    .AsNoTracking()
                    .Select(s => new Advertisement
                    {
                        ID = s.ID,
                        Description = s.Description,
                        Title = s.Title,
                        UserId = s.UserId,
                        GeolocationAds = s.GeolocationAds
                        .Where(gv => DateTime.Today <= gv.ExpirationDate)
                        .Select(gv => new GeolocationAd()
                        {
                            ID = gv.ID,
                            ExpirationDate = gv.ExpirationDate,
                            Latitude = gv.Latitude,
                            Longitude = gv.Longitude,
                            AdvertisingId = gv.AdvertisingId
                        }).ToList(),
                        Settings = s.Settings
                        .Select(s => new AdvertisementSettings()
                        {
                            ID = s.ID,
                            SettingId = s.SettingId,
                            AdvertisementId = s.AdvertisementId,
                            Setting = s.Setting
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
                //var _dataFoundResult = await _context.Advertisements.Include(s => s.Settings)
                //    .Where(v =>
                //    v.UserId == userId &&
                //    v.Settings.Any(s => s.SettingId == typeId))
                //    .OrderByDescending(s => s.CreateDate) // Order by ID (or another suitable property) if needed
                //    .AsSplitQuery()
                //    .Select(s => new Advertisement
                //    {
                //        ID = s.ID,
                //        Description = s.Description,
                //        Title = s.Title,
                //        UserId = s.UserId,
                //        Contents = s.Contents
                //            .Select(ct => new ContentType
                //            {
                //                ID = ct.ID,
                //                Type = ct.Type,
                //                Content = ct.Type == ContentVisualType.Video ? Array.Empty<byte>() : ct.Content,// Apply byte range here
                //                ContentName = ct.ContentName ?? string.Empty,
                //                Url = ct.Type == ContentVisualType.URL ? ct.Url : string.Empty
                //            })
                //            .Take(1)
                //            .ToList()
                //    })
                //    .Skip((pageIndex - 1) * ConstantsTools.PageSize)
                //    .Take(ConstantsTools.PageSize)
                //    .ToListAsync();

                var dataFoundResult = await _context.Advertisements
    .Where(v => v.UserId == userId && v.Settings.Any(s => s.SettingId == typeId))
    .OrderByDescending(s => s.CreateDate) // Order by CreateDate or another suitable property
    .AsNoTracking() // Improve performance by disabling change tracking
    .AsSplitQuery() // Optimize performance for complex queries
    .Select(s => new Advertisement
    {
        ID = s.ID,
        Description = s.Description,
        Title = s.Title,
        UserId = s.UserId,
        CreateDate = s.CreateDate,
        Contents = s.Contents
            .Select(ct => new ContentType
            {
                ID = ct.ID,
                Type = ct.Type,
                Content = ct.Type == ContentVisualType.Video ? Array.Empty<byte>() : ct.Content,
                ContentName = ct.ContentName ?? string.Empty,
                Url = ct.Type == ContentVisualType.URL ? ct.Url : string.Empty,
                FileSize = ct.FileSize,
            })
            .Take(1)
            .ToList(),
        Settings = s.Settings
            .Select(st => new AdvertisementSettings
            {
                ID = st.ID,
                SettingId = st.SettingId,
                AdvertisementId = st.AdvertisementId,
                Setting = st.Setting
            })
            .ToList()
    })
    .Skip((pageIndex - 1) * ConstantsTools.PageSize)
    .Take(ConstantsTools.PageSize)
    .ToListAsync();

                return ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess("Data Found", dataFoundResult, ToolsLibrary.Tools.Type.DataFound);
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

                    return ResponseFactory<Advertisement>.BuildSuccess($"Item {_toRemoved.Title} Removed.", null, ToolsLibrary.Tools.Type.Delete);
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

        public async Task<ResponseTool<bool>> UpdateAd(int Id, Advertisement toUpdate)
        {
            try
            {
                var existingAdvertisement = await _context.Advertisements.Include(s => s.Settings).FirstOrDefaultAsync(v => v.ID == Id);

                if (existingAdvertisement.IsObjectNull())
                    return ResponseFactory<bool>.BuildFail("Advertisement not found.", false, ToolsLibrary.Tools.Type.EntityNotFound);

                // Copy scalar properties
                existingAdvertisement.Title = toUpdate.Title;

                existingAdvertisement.Description = toUpdate.Description;

                existingAdvertisement.Settings = toUpdate.Settings;

                existingAdvertisement.SetUpdateInformation(toUpdate.UpdateBy.Value);

                _context.Entry(existingAdvertisement).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return ResponseFactory<bool>.BuildSuccess("Advertisement updated successfully.", true, ToolsLibrary.Tools.Type.Updated);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.BuildFail(ex.Message, false, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public override async Task<ResponseTool<Advertisement>> UpdateAsync(int id, Advertisement updatedAdvertisement)
        {
            try
            {
                // 🔹 Cargar el anuncio existente sin rastreo para evitar conflictos de seguimiento
                var existingAdvertisement = await _context.Advertisements
                    .AsNoTracking()
                    .Include(a => a.Contents)
                    .Include(a => a.Settings)
                    .FirstOrDefaultAsync(a => a.ID == id);

                if (existingAdvertisement == null)
                {
                    return ResponseFactory<Advertisement>.BuildFail("Advertisement not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
                }

                // 🔹 Actualizar las propiedades escalares
                existingAdvertisement.Title = updatedAdvertisement.Title;

                existingAdvertisement.Description = updatedAdvertisement.Description;

                existingAdvertisement.UpdateDate = DateTime.UtcNow;

                existingAdvertisement.UpdateBy = updatedAdvertisement.UpdateBy;

                // 🔹 Obtener los IDs de los contenidos actualizados
                var updatedContentsIds = updatedAdvertisement.Contents.Select(c => c.ID).ToList();

                // 🔹 Manejar la actualización de la colección de Contents
                foreach (var existingContent in existingAdvertisement.Contents.ToList())
                {
                    var updatedContent = updatedAdvertisement.Contents.FirstOrDefault(c => c.ID == existingContent.ID);

                    if (updatedContent != null && updatedContent.Type == ContentVisualType.Video)
                    {
                        // Mantener la información de creación y actualizar datos
                        updatedContent.CreateDate = existingContent.CreateDate;

                        updatedContent.CreateBy = existingContent.CreateBy;

                        updatedContent.SetUpdateInformation(updatedAdvertisement.UpdateBy.Value);

                        // Obtener el contenido actualizado desde la base de datos si es necesario
                        var _contentBytes = await this.contentTypeRepository.GetContentById(updatedContent.ID);

                        updatedContent.Content = ApiCommonsTools.Combine(_contentBytes.Data);

                        // Aplicar cambios sin añadir una nueva instancia
                        _context.Entry(existingContent).CurrentValues.SetValues(updatedContent);
                    }
                    else
                    {
                        // Eliminar contenido si no existe en la actualización
                        _context.ContentTypes.Remove(existingContent);
                    }
                }

                // 🔹 Agregar nuevos contenidos sin duplicados
                foreach (var newContent in updatedAdvertisement.Contents)
                {
                    if (!existingAdvertisement.Contents.Any(c => c.ID == newContent.ID))
                    {
                        newContent.SetUpdateInformation(updatedAdvertisement.UpdateBy.Value);

                        _context.Attach(newContent);  // Asegurar que EF no duplique el seguimiento

                        existingAdvertisement.Contents.Add(newContent);
                    }
                }

                // 🔹 Reasignar las listas actualizadas
                existingAdvertisement.Contents = updatedAdvertisement.Contents;

                existingAdvertisement.Settings = updatedAdvertisement.Settings;

                // 🔹 Marcar la entidad como modificada
                _context.Entry(existingAdvertisement).State = EntityState.Modified;

                // 🔹 Guardar cambios
                await _context.SaveChangesAsync();

                return ResponseFactory<Advertisement>.BuildSuccess("Advertisement updated successfully.", existingAdvertisement, ToolsLibrary.Tools.Type.Updated);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task UpdateContentsAsync(IEnumerable<ContentType> existingContents, IEnumerable<ContentType> updatedContents, int updatedByUserId, int advertisingId)
        {
            foreach (var existingItem in existingContents)
            {
                var updatedItem = updatedContents.FirstOrDefault(u => u.ID == existingItem.ID);

                if (updatedItem != null)
                {
                    await UpdateExistingContentAsync(existingItem, updatedItem, updatedByUserId, advertisingId);
                }
                else
                {
                    SetNewContentProperties(existingItem, updatedByUserId, advertisingId);
                }

                _context.Entry(existingItem).State = EntityState.Modified;
            }
        }

        private async Task UpdateExistingContentAsync(ContentType existingItem, ContentType updatedItem, int updatedByUserId, int advertisingId)
        {
            if (!existingItem.IsObjectNull() && existingItem.Type == ContentVisualType.Video && Convert.ToUInt64(existingItem.FileSize) == 0)
            {
                existingItem.SetUpdateInformation(updatedByUserId);

                existingItem.AdvertisingId = advertisingId;

                var contentBytes = await this.contentTypeRepository.GetContentById(existingItem.ID);

                existingItem.Content = ApiCommonsTools.Combine(contentBytes.Data);
            }
        }

        private void SetNewContentProperties(ContentType existingItem, int updatedByUserId, int advertisingId)
        {
            existingItem.CreateDate = DateTime.Now;

            existingItem.CreateBy = updatedByUserId;

            existingItem.AdvertisingId = advertisingId;
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