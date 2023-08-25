using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class GoogleMapPage : ContentPage
{
    private GoogleMapViewModel _viewModel;

    public GoogleMapPage(GoogleMapViewModel googleMapViewModel)
    {
        InitializeComponent();

        this._viewModel = googleMapViewModel;

        BindingContext = googleMapViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.Initialize();

        var _pinData = this._viewModel.GetContentPins();

        foreach (var item in _pinData)
        {
            this.googleMap.Pins.Add(item);
        }
    }
}