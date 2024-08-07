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

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();

    //    //this._viewModel.IsLoading = true;

    //    //await _viewModel.Initialize();

    //    //var _pinData = this._viewModel.GetContentPins();

    //    //this.googleMap.Pins.AddRange(_pinData);

    //    //var _mapChildren = this.mapContainer.Children.Where(v => v.GetType() == typeof(Map)).FirstOrDefault();

    //    //if (!_mapChildren.IsObjectNull())
    //    //{
    //    //    this.mapContainer.Children.Remove(_mapChildren);
    //    //}

    //    //var _currentLocation = await GeolocationTool.GetLocation();

    //    //if (_currentLocation.IsSuccess)
    //    //{
    //    //    MapSpan mapSpan = new MapSpan(_currentLocation.Data, 0.01, 0.01);

    //    //    Map myMap = new Map(mapSpan);

    //    //    myMap.IsScrollEnabled = true;

    //    //    myMap.IsShowingUser = true;

    //    //    myMap.IsZoomEnabled = true;

    //    //    myMap.MapType = MapType.Street;

    //    //    await _viewModel.Initialize();

    //    //    var _pinData = this._viewModel.GetContentPins();

    //    //    myMap.Pins.AddRange(_pinData);

    //    //    this.mapContainer.Children.Add(myMap);
    //    //}
    //    //else
    //    //{
    //    //    await Shell.Current.DisplayAlert("Error", _currentLocation.Message, "OK");
    //    //}

    //    this._viewModel.IsLoading = false;
    //}
}