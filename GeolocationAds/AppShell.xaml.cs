using GeolocationAds.Pages;
using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel2 appShellViewModel)
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Register), typeof(Register));

        Routing.RegisterRoute(nameof(SearchAd), typeof(SearchAd));

        Routing.RegisterRoute(nameof(Login), typeof(Login));

        Routing.RegisterRoute(nameof(EditAdvertisment), typeof(EditAdvertisment));

        Routing.RegisterRoute(nameof(NearByItemDetail), typeof(NearByItemDetail));

        Routing.RegisterRoute(nameof(EditUserPerfil), typeof(EditUserPerfil));

        Routing.RegisterRoute(nameof(EditLoginCredential), typeof(EditLoginCredential));

        Routing.RegisterRoute(nameof(ManageLocation), typeof(ManageLocation));

        BindingContext = appShellViewModel;
    }
}