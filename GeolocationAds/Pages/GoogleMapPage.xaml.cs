using GeolocationAds.ViewModels;
using ToolsLibrary.Extensions;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class GoogleMapPage : ContentPage
{
    private GoogleMapViewModel2 _viewModel;

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

            this.googleMap.Pins.Clear();

            var _pinData = this._viewModel.GetContentPins();

            this.googleMap.Pins.AddRange(_pinData);
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
        base.OnAppearing();

        this._viewModel.IsLoading = true;

        await _viewModel.InitializeAsync();

        var _pinData = this._viewModel.GetContentPins();

        this.googleMap.Pins.AddRange(_pinData);

        //var _mapChildren = this.mapContainer.Children.Where(v => v.GetType() == typeof(Map)).FirstOrDefault();

        //if (!_mapChildren.IsObjectNull())
        //{
        //    this.mapContainer.Children.Remove(_mapChildren);
        //}

        //var _currentLocation = await GeolocationTool.GetLocation();

        //if (_currentLocation.IsSuccess)
        //{
        //    MapSpan mapSpan = new MapSpan(_currentLocation.Data, 0.01, 0.01);

        //    Map myMap = new Map(mapSpan);

        //    myMap.IsScrollEnabled = true;

        //    myMap.IsShowingUser = true;

        //    myMap.IsZoomEnabled = true;

        //    myMap.MapType = MapType.Street;

        //    await _viewModel.Initialize();

        //    var _pinData = this._viewModel.GetContentPins();

        //    myMap.Pins.AddRange(_pinData);

        //    this.mapContainer.Children.Add(myMap);
        //}
        //else
        //{
        //    await Shell.Current.DisplayAlert("Error", _currentLocation.Message, "OK");
        //}

        this._viewModel.IsLoading = false;
    }
}