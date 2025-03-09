using GeolocationAds.Tools;
using GeolocationAds.ViewModels;
using Microsoft.Maui.Maps;

namespace GeolocationAds.Pages;

public partial class ManageLocation : ContentPage
{
    public ManageLocation(ManageLocationViewModel2 manageLocationViewModel)
    {
        InitializeComponent();

        this.BindingContext = manageLocationViewModel;
    }

    private void googleMap_MapClicked(object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
    }

    protected override async void OnAppearing()
    {
        try
        {
            var _currentLocation = await GeolocationTool.GetLocation();

            if (!_currentLocation.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Error", _currentLocation.Message, "OK");
            }

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(_currentLocation.Data, Distance.FromMiles(0.1));

            this.myMap.MoveToRegion(mapSpan);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}