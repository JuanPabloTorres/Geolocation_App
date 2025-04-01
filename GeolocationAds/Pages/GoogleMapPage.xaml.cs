using GeolocationAds.Tools;
using GeolocationAds.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
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

        this._viewModel.PinsUpdated = _viewModel_PinsUpdated;
    }

    private async void _viewModel_PinsUpdated(ObservableCollection<Pin> sender)
    {
        await _viewModel.RunWithLoadingIndicator(async () =>
        {
            this.myMap.Pins.Clear();

            this.myMap.Pins.AddRange(sender);
        });
    }

    protected override async void OnAppearing()
    {
        await _viewModel.RunWithLoadingIndicator(async () =>
        {
            var locationResult = await GeolocationTool.GetLocation();

            if (!locationResult.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Error", locationResult.Message, "OK");
                return;
            }

            var existingMap = mapContainer.Children.FirstOrDefault(v => v is Map);

            if (existingMap != null)
                mapContainer.Children.Remove(existingMap);

            var mapSpan = MapSpan.FromCenterAndRadius(locationResult.Data, Distance.FromMiles(0.1));

            myMap = new Map(mapSpan)
            {
                IsScrollEnabled = true,
                IsShowingUser = true,
                IsZoomEnabled = true,
                MapType = MapType.Hybrid
            };

            await _viewModel.InitializeAsync();

            var pinData = _viewModel.GetContentPins();

            myMap.Pins.AddRange(pinData);

            mapContainer.Children.Add(myMap);
        });
    }
}