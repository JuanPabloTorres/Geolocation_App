using GeolocationAds.Pages;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;

namespace GeolocationAds;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");

                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                fonts.AddFont("Roboto-Light.ttf", "Roboto");

                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
            }).UseMauiMaps();



        builder.Services.AddTransient<IGeolocationAdService, GeolocationAdService>();

        builder.Services.AddTransient<IAdvertisementService, AdvertisementService>();

        builder.Services.AddTransient<ILoginService, LoginService>();

        builder.Services.AddTransient<IUserService, UserService>();

        builder.Services.AddTransient<User>();

        builder.Services.AddTransient<ToolsLibrary.Models.Login>();

        builder.Services.AddSingleton<CreateGeolocationViewModel>();

        builder.Services.AddSingleton<AdLocationTemplateViewModel>();

        builder.Services.AddSingleton<AdToLocationViewModel>();

        builder.Services.AddSingleton<SearchAdViewModel>();

        builder.Services.AddSingleton<LoginViewModel>();

        builder.Services.AddSingleton<CreateAdvertisment>();

        builder.Services.AddSingleton<RegisterViewModel>();

        builder.Services.AddSingleton<AdToLocation>();

        builder.Services.AddSingleton<SearchAd>();

        builder.Services.AddSingleton<Login>();

        builder.Services.AddSingleton<Register>();

        return builder.Build();
    }
}