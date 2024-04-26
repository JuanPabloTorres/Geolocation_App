using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class Login : ContentPage
{
    public Login(LoginViewModel2 loginViewModel)
    {
        InitializeComponent();

        BindingContext = loginViewModel;
    }
}