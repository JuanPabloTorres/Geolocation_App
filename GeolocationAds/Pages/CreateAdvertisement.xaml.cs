using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    CreateAdvertismentViewModel viewModel;

    public CreateAdvertisment(CreateAdvertismentViewModel createGeolocationViewModel)
    {
        InitializeComponent();

        viewModel = createGeolocationViewModel;

        BindingContext = createGeolocationViewModel;
    }

    protected async override void OnAppearing()
    {
        await viewModel.LoadSetting();
    }


}