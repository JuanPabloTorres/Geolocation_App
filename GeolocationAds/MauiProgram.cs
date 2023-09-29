using CommunityToolkit.Maui;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
using Microsoft.Maui.Controls.Maps;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkitMediaElement()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");

                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                fonts.AddFont("Roboto-Light.ttf", "Roboto");

                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");

                fonts.AddFont("Sunshine.ttf", "Sunshine");
            }).UseMauiMaps();

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
        {
#if ANDROID

            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
        {
#if ANDROID

            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });

        #region Api Service

        builder.Services.AddTransient<IGeolocationAdService, GeolocationAdService>();

        builder.Services.AddTransient<IAdvertisementService, AdvertisementService>();

        builder.Services.AddTransient<ILoginService, LoginService>();

        builder.Services.AddTransient<IUserService, UserService>();

        builder.Services.AddTransient<IAppSettingService, AppSettingService>();

        builder.Services.AddTransient<IForgotPasswordService, ForgotPasswordService>();

        builder.Services.AddTransient<ICaptureService, CaptureService>();

        #endregion Api Service

        #region Models

        builder.Services.AddTransient<User>();

        builder.Services.AddTransient<Pin>();

        builder.Services.AddTransient<ToolsLibrary.Models.Login>();

        builder.Services.AddTransient<ToolsLibrary.Models.Advertisement>();

        builder.Services.AddTransient<ToolsLibrary.Models.Capture>();

        #endregion Models

        #region ViewModels

        builder.Services.AddScoped<CreateAdvertismentViewModel>();

        builder.Services.AddScoped<AdLocationTemplateViewModel>();

        builder.Services.AddScoped<AdToLocationViewModel>();

        builder.Services.AddScoped<SearchAdViewModel>();

        builder.Services.AddScoped<LoginViewModel>();

        builder.Services.AddScoped<RegisterViewModel>();

        builder.Services.AddScoped<AppShellViewModel>();

        builder.Services.AddScoped<GoogleMapViewModel>();

        builder.Services.AddScoped<EditAdvertismentViewModel>();

        builder.Services.AddScoped<UserSettingViewModel>();

        builder.Services.AddScoped<EditUserPerfilViewModel>();

        builder.Services.AddScoped<EditLoginCredentialViewModel>();

        builder.Services.AddScoped<RecoveryPasswordViewModel>();

        builder.Services.AddScoped<CaptureViewModel>();

        #endregion ViewModels

        #region Pages

        builder.Services.AddScoped<CreateAdvertisment>();

        //builder.Services.AddTransientWithShellRoute<CreateAdvertisment, CreateAdvertismentViewModel>($"{nameof(CreateAdvertisment)}");

        builder.Services.AddSingleton<AdToLocation>();

        builder.Services.AddSingleton<SearchAd>();

        builder.Services.AddSingleton<Login>();

        builder.Services.AddSingleton<Register>();

        builder.Services.AddSingleton<GoogleMapPage>();

        builder.Services.AddSingleton<UserSetting>();

        builder.Services.AddTransientWithShellRoute<EditAdvertisment, EditAdvertismentViewModel>($"{nameof(EditAdvertisment)}");

        builder.Services.AddSingleton<EditUserPerfil>();

        builder.Services.AddSingleton<EditLoginCredential>();

        builder.Services.AddSingleton<RecoveryPasswordPopUp>();

        builder.Services.AddSingleton<MyFavorites>();

        #endregion Pages

        #region Tools

        builder.Services.AddSingleton<LogUserPerfilTool>();

        #endregion Tools

        return builder.Build();
    }
}