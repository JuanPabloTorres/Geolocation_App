using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    public CreateAdvertisment(CreateAdvertismentViewModel createGeolocationViewModel)
    {
        InitializeComponent();

        BindingContext = createGeolocationViewModel;
    }


}