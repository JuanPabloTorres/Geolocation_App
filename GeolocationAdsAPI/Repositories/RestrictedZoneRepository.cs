using GeolocationAdsAPI.Context;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public class RestrictedZoneRepository : BaseRepositoryImplementation<RestrictedZone>, IRestrictedZoneRepository

    {
        public RestrictedZoneRepository(GeolocationContext context) : base(context)
        {
        }

        public async Task<ResponseTool<IEnumerable<RestrictedZone>>> GetAllActiveRestrictedZonesAsync()
        {
            try
            {
                var zones = await _context.RestrictedZones
                    .AsNoTracking()
                    .Where(z => z.IsActive)
                    .ToListAsync();

                if (!zones.Any())
                {
                    return ResponseFactory<IEnumerable<RestrictedZone>>.BuildFail(
                        "No active restricted zones found.",
                        zones,
                        ToolsLibrary.Tools.Type.EmptyCollection);
                }

                return ResponseFactory<IEnumerable<RestrictedZone>>.BuildSuccess(
                    "Restricted zones loaded successfully.",
                    zones,
                    ToolsLibrary.Tools.Type.DataFound);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<RestrictedZone>>.BuildFail(
                    $"Error loading restricted zones: {ex.Message}",
                    null,
                    ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}