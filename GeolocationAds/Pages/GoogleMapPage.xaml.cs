using ToolsLibrary.Models;
using Microsoft.Maui.Maps;


namespace GeolocationAds.Pages;

public partial class GoogleMapPage : ContentPage
{
    public IList<GeolocationAd> GeolocationRegistered = new List<GeolocationAd>();

    public IList<Location> FoundLocations = new List<Location>();

    public GoogleMapPage()
    {
        InitializeComponent();

        Task.Run(async () => { await GetLocation(); }).Wait();
    }

    public GoogleMapPage(IList<GeolocationAd> geoLocationRegistered, IList<Location> foundLocations)
    {
        InitializeComponent();

        Task.Run(async () => { await GetLocation(); }).Wait();

        this.GeolocationRegistered = geoLocationRegistered;

        this.FoundLocations = foundLocations;

        SetRegisteredLocationsPins(GeolocationRegistered);

        SetFoundLocationsPins(FoundLocations);
    }

    private void SetRegisteredLocationsPins(IList<GeolocationAd> geoLocations)
    {
        foreach (var adsGeoItem in geoLocations)
        {
            Location location = new Location()
            {
                Latitude = adsGeoItem.Latitude,
                Longitude = adsGeoItem.Longitude,
            };

            googleMap.Pins.Add(new Microsoft.Maui.Controls.Maps.Pin()
            {
                Location = location,
                Label = adsGeoItem.Advertisement.Title,
                Address = $"LA:{location.Latitude} Lo:{location.Longitude}",
                Type = Microsoft.Maui.Controls.Maps.PinType.Place,
            });
        }
    }

    private void SetFoundLocationsPins(IList<Location> foundLocations)
    {
        int index = 1;

        foreach (var location in foundLocations)
        {
            googleMap.Pins.Add(new Microsoft.Maui.Controls.Maps.Pin()
            {
                Location = location,
                Type = Microsoft.Maui.Controls.Maps.PinType.SearchResult,
                Address = $"LA:{location.Latitude} Lo:{location.Longitude}",
                Label = $"Location #{index}"
            });

            index++;
        }
    }

    private async Task GetLocation()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));

        var location = await Geolocation.GetLocationAsync(request);

        if (location != null)
        {
            googleMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(1)));
        }
        else
        {
        }
    }
}