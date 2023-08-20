using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class App : Application
{
    public App(LoginViewModel loginViewModel)
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}