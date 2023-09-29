using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class MyFavorites : ContentPage
{
    private CaptureViewModel viewModel;

    public MyFavorites(CaptureViewModel captureViewModel)
    {
        InitializeComponent();

        viewModel = captureViewModel;

        this.BindingContext = captureViewModel;
    }

    protected override void OnAppearing()
    {
        this.viewModel.Initialize();
    }
}