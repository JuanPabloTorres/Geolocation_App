using GeolocationAds.Tools;
using GeolocationAds.ViewModels;
using Microsoft.Maui.Maps;
using ToolsLibrary.Extensions;
using ToolsLibrary.Tools;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace GeolocationAds.Pages;

public partial class GoogleMapPage : ContentPage
{
    private GoogleMapViewModel2 _viewModel;

    private Map myMap;

    public GoogleMapPage(GoogleMapViewModel2 googleMapViewModel)
    {
        InitializeComponent();

        this._viewModel = googleMapViewModel;

        BindingContext = googleMapViewModel;

        this._viewModel.PinsUpdated += _viewModel_PinsUpdated;
    }

    private async void _viewModel_PinsUpdated(object sender, EventArgs e)
    {
        try
        {
            this._viewModel.IsLoading = true;

            this.myMap.Pins.Clear();

            var _pinData = this._viewModel.GetContentPins();

            this.myMap.Pins.AddRange(_pinData);
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
        finally
        {
            this._viewModel.IsLoading = false;
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            this._viewModel.IsLoading = true;

            var _currentLocation = await GeolocationTool.GetLocation();

            if (_currentLocation.IsSuccess)
            {
                var _mapChildren = this.mapContainer.Children.Where(v => v.GetType() == typeof(Map)).FirstOrDefault();

                if (!_mapChildren.IsObjectNull())
                {
                    this.mapContainer.Children.Remove(_mapChildren);
                }

                MapSpan mapSpan = MapSpan.FromCenterAndRadius(_currentLocation.Data, Distance.FromMiles(0.1));

                this.myMap = new Map(mapSpan)
                {
                    IsScrollEnabled = true,

                    IsShowingUser = true,

                    IsZoomEnabled = true,

                    MapType = MapType.Hybrid
                };

                await _viewModel.InitializeAsync();

                var _pinData = this._viewModel.GetContentPins();

                this.myMap.Pins.AddRange(_pinData);

                this.mapContainer.Children.Add(myMap);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _currentLocation.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            this._viewModel.IsLoading = false;
        }
    }
}