using GeolocationAds.Pages;
using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel appShellViewModel)
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Register), typeof(Register));

        Routing.RegisterRoute(nameof(Login), typeof(Login));

        Routing.RegisterRoute(nameof(EditAdvertisment), typeof(EditAdvertisment));

        Routing.RegisterRoute(nameof(EditUserPerfil), typeof(EditUserPerfil));

        BindingContext = appShellViewModel;
    }


}