using GeolocationAds.Pages;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
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

                //fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
            }).UseMauiMaps();

        // Create a custom HttpClientHandler to allow self-signed certificates
        //var httpClientHandler = new HttpClientHandler();
        //httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        //{
        //    // Add your validation logic here, for development/testing purposes,
        //    // you can simply return true to trust any certificate, but this is not recommended for production use.
        //    return true;
        //};

        //// Use the custom HttpClientHandler in HttpClient
        //var httpClient = new HttpClient(httpClientHandler);

        //// Register the HttpClient as a singleton service in the service container
        //builder.Services.AddSingleton(httpClient);

        builder.Services.AddTransient<IGeolocationAdService, GeolocationAdService>();

        builder.Services.AddTransient<IAdvertisementService, AdvertisementService>();

        builder.Services.AddTransient<CreateGeolocationViewModel>();

        builder.Services.AddTransient<AdLocationTemplateViewModel>();

        builder.Services.AddTransient<AdToLocationViewModel>();

        builder.Services.AddTransient<SearchAdViewModel>();

        builder.Services.AddTransient<CreateAdvertisment>();

        builder.Services.AddTransient<AdToLocation>();

        builder.Services.AddTransient<SearchAd>();

        return builder.Build();
    }
}