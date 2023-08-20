using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class Login : ContentPage
{
    public Login(LoginViewModel loginViewModel)
    {
        InitializeComponent();

        BindingContext = loginViewModel;
    }
}