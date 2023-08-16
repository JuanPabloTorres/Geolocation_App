using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class AdToLocation : ContentPage
{
    private AdToLocationViewModel viewModel;

    public AdToLocation(AdToLocationViewModel adToLocationViewModel)
    {
        InitializeComponent();

        this.viewModel = adToLocationViewModel;

        BindingContext = adToLocationViewModel;
    }
}