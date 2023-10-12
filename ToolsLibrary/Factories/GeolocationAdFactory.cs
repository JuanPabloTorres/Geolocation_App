using ToolsLibrary.Models;

namespace ToolsLibrary.Factories
{
    public class GeolocationAdFactory
    {
        public static GeolocationAd CreateGeolocationAd(Advertisement ad, Location locationData)
        {
            return new GeolocationAd()
            {
                AdvertisingId = ad.ID,
                Advertisement = ad,
                CreateDate = DateTime.Now,
                Latitude = locationData.Latitude,
                Longitude = locationData.Longitude,
                ExpirationDate = DateTime.Now.AddDays(7)
            };
        }
    }
}