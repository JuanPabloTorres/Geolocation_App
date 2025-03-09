using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.DependencyInjection;
using FFImageLoading.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using GeolocationAds.Factories;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Maps;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ToolsLibrary.Models;
using ToolsLibrary.Models.Settings;
using ToolsLibrary.Tools;

namespace GeolocationAds;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseFFImageLoading()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .UseSkiaSharp()
            .UseMauiMaps()
            .ConfigureFonts(ConfigureAppFonts);

        // Cargar configuración desde el archivo de configuración
        var configuration = CommonsTool.ConfigurationLoader.LoadConfiguration();

        builder.Services.AddSingleton<IConfiguration>(configuration);

        // Registrar dependencias
        RegisterApiServices(builder);

        RegisterModels(builder);

        RegisterViewModels(builder);

        RegisterContainers(builder);

        RegisterPages(builder);

        RegisterTools(builder,configuration);

        RegisterFactories(builder);

        return builder.Build();
    }

    #region **Configuración de Fuentes**

    private static void ConfigureAppFonts(IFontCollection fonts)
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        fonts.AddFont("Roboto-Light.ttf", "Roboto");
        fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
        fonts.AddFont("Sunshine.ttf", "Sunshine");
        fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIconsOutlined-Regular");
        fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons-Regular");
    }

    #endregion **Configuración de Fuentes**

    #region **Registro de Servicios**

    private static void RegisterApiServices(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IGeolocationAdService, GeolocationAdService>();
        builder.Services.AddTransient<IAdvertisementService, AdvertisementService>();
        builder.Services.AddTransient<ILoginService, LoginService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IAppSettingService, AppSettingService>();
        builder.Services.AddTransient<IForgotPasswordService, ForgotPasswordService>();
        builder.Services.AddTransient<ICaptureService, CaptureService>();
        builder.Services.AddTransient<IFirebaseAuthService, FirebaseAuthService>();
        builder.Services.AddTransient<IGoogleAuthService, GoogleAuthService>();

        builder.Services.AddSingleton<HttpClient>(provider =>
        {
            return new HttpClient()
            {
                Timeout = TimeSpan.FromMinutes(ConstantsTools.TIMEOUT)
            };
        });
    }

    #endregion **Registro de Servicios**

    #region **Registro de Modelos**

    private static void RegisterModels(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ToolsLibrary.Models.User>();
        builder.Services.AddTransient<Pin>();
        builder.Services.AddTransient<ToolsLibrary.Models.Login>();
        builder.Services.AddTransient<ToolsLibrary.Models.Advertisement>();
        builder.Services.AddTransient<ToolsLibrary.Models.Capture>();
    }

    #endregion **Registro de Modelos**

    #region **Registro de ViewModels**

    private static void RegisterViewModels(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<CreateAdvertismentViewModel2>();
        builder.Services.AddScoped<MangeContentTemplateViewModel>();
        builder.Services.AddScoped<MyContentViewModel2>();
        builder.Services.AddScoped<SearchAdViewModel2>();
        builder.Services.AddScoped<LoginViewModel2>();
        builder.Services.AddScoped<RegisterViewModel>();
        builder.Services.AddScoped<AppShellViewModel2>();
        builder.Services.AddScoped<GoogleMapViewModel2>();
        builder.Services.AddScoped<EditAdvertismentViewModel2>();
        builder.Services.AddScoped<UserSettingViewModel>();
        builder.Services.AddScoped<EditUserPerfilViewModel>();
        builder.Services.AddScoped<EditLoginCredentialViewModel>();
        builder.Services.AddScoped<RecoveryPasswordViewModel>();
        builder.Services.AddScoped<FilterPopUpViewModel>();
        builder.Services.AddScoped<FilterPopUpViewModel2>();
        builder.Services.AddScoped<CaptureViewModel2>();
        builder.Services.AddScoped<ManageLocationViewModel>();
        builder.Services.AddScoped<ManageLocationViewModel2>();
        builder.Services.AddScoped<ContentViewTemplateViewModel>();
        builder.Services.AddScoped<NearByItemDetailViewModel>();
        builder.Services.AddScoped<FacebookAuthWebViewViewModel>();
        builder.Services.AddScoped<GoogleAuthWebViewViewModel>();
    }

    #endregion **Registro de ViewModels**

    #region **Registro de Contenedores**

    private static void RegisterContainers(MauiAppBuilder builder)
    {
        builder.Services.AddScoped<IContainerMyContentServices, ContainerMyContentServices>();
        builder.Services.AddScoped<IContainerMapServices, ContainerMapServices>();
        builder.Services.AddScoped<IContainerEditAdvertisment, ContainerEditAdvertisment>();
        builder.Services.AddScoped<IContainerCreateAdvertisment, ContainerCreateAdvertisment>();
        builder.Services.AddScoped<INearByItemDetailContainer, NearByItemDetailContainer>();
        builder.Services.AddScoped<IContainerManageLocation, ContainerManageLocation>();
        builder.Services.AddScoped<IContainerCapture, ContainerCapture>();
        builder.Services.AddScoped<IContainerLoginServices, ContainerLoginServices>();
    }

    #endregion **Registro de Contenedores**

    #region **Registro de Páginas**

    private static void RegisterPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<FilterPopUp>();

        builder.Services.AddTransientWithShellRoute<CreateAdvertisment, CreateAdvertismentViewModel2>($"{nameof(CreateAdvertisment)}");
        builder.Services.AddScoped<SearchAd>();

        builder.Services.AddScoped<MyContentPage>();

        builder.Services.AddTransient<Login>();

        builder.Services.AddScoped<Register>();

        builder.Services.AddScoped<GoogleMapPage>();

        builder.Services.AddScoped<UserSetting>();

        builder.Services.AddTransientWithShellRoute<EditAdvertisment, EditAdvertismentViewModel2>($"{nameof(EditAdvertisment)}");

        builder.Services.AddTransientWithShellRoute<NearByItemDetail, NearByItemDetailViewModel>($"{nameof(NearByItemDetail)}");

        builder.Services.AddScoped<EditUserPerfil>();

        builder.Services.AddScoped<EditLoginCredential>();

        builder.Services.AddScoped<RecoveryPasswordPopUp>();

        builder.Services.AddScoped<MyFavorites>();

        builder.Services.AddScopedWithShellRoute<FacebookAuthWebViewPage, FacebookAuthWebViewViewModel>($"{nameof(FacebookAuthWebViewPage)}");

        builder.Services.AddScopedWithShellRoute<GoogleAuthWebViewPage, GoogleAuthWebViewViewModel>($"{nameof(GoogleAuthWebViewPage)}");

        builder.Services.AddTransientWithShellRoute<ManageLocation, ManageLocationViewModel2>($"{nameof(ManageLocation)}");
    }

    #endregion **Registro de Páginas**

    #region **Registro de Herramientas**

    private static void RegisterTools(MauiAppBuilder builder,IConfiguration configuration)
    {
        builder.Services.AddSingleton<LogUserPerfilTool>();

        builder.Services.AddScoped<ISecureStoreService, SecureStoreService>();

        // Configurar GoogleAuthSettings usando Bind
        builder.Services.Configure<GoogleAuthSettings>(options => configuration.GetSection("GoogleSettings").Bind(options));

        //builder.Services.AddSingleton<GoogleAuthService>();
    }

    #endregion **Registro de Herramientas**

    #region **Registro de Fábricas**

    private static void RegisterFactories(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ILoginFactory, LoginFactory>();
        builder.Services.AddSingleton<IUserFactory, UserFactory>();
    }

    #endregion **Registro de Fábricas**
}