using GeolocationAds.Pages;
using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel2 appShellViewModel)
    {
        InitializeComponent();

        // Registrar todas las rutas de navegación
        RegisterRoutes();

        // Configurar el BindingContext para el ViewModel
        BindingContext = appShellViewModel;
    }

    /// <summary>
    /// Registra las rutas de navegación dentro de la aplicación.
    /// </summary>
    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(Register), typeof(Register));

        Routing.RegisterRoute(nameof(SearchAd), typeof(SearchAd));

        Routing.RegisterRoute(nameof(Login), typeof(Login));
      
        Routing.RegisterRoute(nameof(CreateAdvertisment), typeof(CreateAdvertisment));
        
        Routing.RegisterRoute(nameof(UserSetting), typeof(UserSetting));

        Routing.RegisterRoute(nameof(EditAdvertisment), typeof(EditAdvertisment));

        Routing.RegisterRoute(nameof(NearByItemDetail), typeof(NearByItemDetail));

        Routing.RegisterRoute(nameof(EditUserPerfil), typeof(EditUserPerfil));

        Routing.RegisterRoute(nameof(EditLoginCredential), typeof(EditLoginCredential));

        Routing.RegisterRoute(nameof(ManageLocation), typeof(ManageLocation));

        Routing.RegisterRoute(nameof(FacebookAuthWebViewPage), typeof(FacebookAuthWebViewPage));
    }
}