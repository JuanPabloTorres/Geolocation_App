using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        var viewModel = new AppShellViewModel();

        MainPage = new AppShell(viewModel);
    }
}