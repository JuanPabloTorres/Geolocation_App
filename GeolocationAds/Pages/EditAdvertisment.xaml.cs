using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditAdvertisment : ContentPage
{
    public EditAdvertisment(EditAdvertismentViewModel editAdvertismentViewModel)
    {
        InitializeComponent();

        BindingContext = editAdvertismentViewModel;
    }
}