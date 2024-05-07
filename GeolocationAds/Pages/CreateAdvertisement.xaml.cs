using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    private CreateAdvertismentViewModel2 viewModel;

    public CreateAdvertisment(CreateAdvertismentViewModel2 createGeolocationViewModel)
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