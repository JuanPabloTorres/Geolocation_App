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

        await manageLocationViewModel2.CreateAdToLocation(clickedLocation);
    }

    protected override async void OnAppearing()
    {
        await manageLocationViewModel2.RunWithLoadingIndicator(async () =>
        {
            var _currentLocation = await GeolocationTool.GetLocation();

            if (!_currentLocation.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Error", _currentLocation.Message, "OK");

                return;
            }

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(_currentLocation.Data, Distance.FromMiles(0.1));

            this.myMap.MoveToRegion(mapSpan);
        });
    }
}