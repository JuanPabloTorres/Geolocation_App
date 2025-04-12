using GeolocationAds.Tools;
using GeolocationAds.ViewModels;
using Microsoft.Maui.Maps;

namespace GeolocationAds.Pages;

public partial class ManageLocation : ContentPage
{
    private ManageLocationViewModel2 manageLocationViewModel2;

    public ManageLocation(ManageLocationViewModel2 manageLocationViewModel)
    {
        InitializeComponent();

        manageLocationViewModel2 = manageLocationViewModel;

        this.BindingContext = manageLocationViewModel2;
    }

    private async void googleMap_MapClicked(object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        var clickedLocation = e.Location;

        // Show confirmation alert
        bool confirm = await Shell.Current.DisplayAlert(
            "Add Location",
            $"Do you want to pin this location?\n\nLatitude: {clickedLocation.Latitude:F5}\nLongitude: {clickedLocation.Longitude:F5}",
            "Yes",
            "Cancel");

        if (confirm)
        {
            await manageLocationViewModel2.CreateAdToLocation(clickedLocation);
        }
    }

    protected override async void OnAppearing()
    {
        await manageLocationViewModel2.RunWithLoadingIndicator(async () =>
        {
            var _currentLocationResponse = await GeolocationTool.GetLocation();

            if (!_currentLocationResponse.IsSuccess)
            {
                throw new Exception(_currentLocationResponse.Message);
            }

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(_currentLocationResponse.Data, Distance.FromMiles(0.1));

            this.myMap.MoveToRegion(mapSpan);
        });
    }
}