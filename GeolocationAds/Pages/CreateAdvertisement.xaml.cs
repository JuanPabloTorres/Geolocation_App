using GeolocationAds.ViewModels;
using ToolsLibrary.Extensions;

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

    protected override void OnAppearing()
    {
        //await viewModel.LoadSetting();

        if (this.viewModel.AdTypesSettings.Count() > 0 && !this.viewModel.AdTypesSettings.IsObjectNull())
        {
            this.viewModel.SetDefault();
        }
    }
}