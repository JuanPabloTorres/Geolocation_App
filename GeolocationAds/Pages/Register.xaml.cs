using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class Register : ContentPage
{
    public Register(RegisterViewModel registerViewModel)
    {
        InitializeComponent();

        BindingContext = registerViewModel;
    }
}