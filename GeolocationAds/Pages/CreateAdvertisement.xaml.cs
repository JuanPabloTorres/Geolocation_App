using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    private CreateAdvertismentViewModel viewModel;

    public CreateAdvertisment(CreateAdvertismentViewModel createGeolocationViewModel)
    {
        InitializeComponent();

        viewModel = createGeolocationViewModel;

        BindingContext = createGeolocationViewModel;
    }

    protected override async void OnAppearing()
    {
        if (this.viewModel.AdTypesSettings.Count() == 0)
        {
            await this.viewModel.InitializeSettings();

            this.viewModel.SetDefault();
        }
        else
        {
            this.viewModel.SetDefault();
        }
    }
}