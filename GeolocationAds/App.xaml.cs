using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        var viewModel = new AppShellViewModel2();

        MainPage = new AppShell(viewModel);
    }
}