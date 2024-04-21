using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
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
             .UseFFImageLoading()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");

                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                fonts.AddFont("Roboto-Light.ttf", "Roboto");

                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");

                fonts.AddFont("Sunshine.ttf", "Sunshine");

                fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIconsOutlined-Regular");

                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons-Regular");
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

        builder.Services.AddSingleton<HttpClient>((provider) =>
        {
            var _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(ConstantsTools.TIMEOUT)
            };

            return _httpClient;
        });

        #endregion Api Service

        #region Models

        builder.Services.AddTransient<User>();

        builder.Services.AddTransient<Pin>();

        builder.Services.AddTransient<ToolsLibrary.Models.Login>();

        builder.Services.AddTransient<ToolsLibrary.Models.Advertisement>();

        builder.Services.AddTransient<ToolsLibrary.Models.Capture>();

        #endregion Models

        #region ViewModels

        builder.Services.AddTransient<CreateAdvertismentViewModel>();

        builder.Services.AddScoped<MangeContentTemplateViewModel>();

        builder.Services.AddScoped<MyContentViewModel>();

        builder.Services.AddScoped<SearchAdViewModel>();
        
        builder.Services.AddScoped<SearchAdViewModel2>();

        builder.Services.AddScoped<LoginViewModel>();

        builder.Services.AddScoped<RegisterViewModel>();

        builder.Services.AddScoped<AppShellViewModel>();

        builder.Services.AddScoped<GoogleMapViewModel>();
        
        builder.Services.AddScoped<GoogleMapViewModel2>();

        builder.Services.AddScoped<EditAdvertismentViewModel>();

        builder.Services.AddScoped<UserSettingViewModel>();

        builder.Services.AddScoped<EditUserPerfilViewModel>();

        builder.Services.AddScoped<EditLoginCredentialViewModel>();

        builder.Services.AddScoped<RecoveryPasswordViewModel>();

        builder.Services.AddScoped<FilterPopUpViewModel>();

        builder.Services.AddScoped<CaptureViewModel>();

        builder.Services.AddScoped<ManageLocationViewModel>();
        
        builder.Services.AddScoped<ContentViewTemplateViewModel>();

        #endregion ViewModels

        #region Pages

        builder.Services.AddTransient<FilterPopUp>();

        //builder.Services.AddSingleton<CreateAdvertisment>();

        builder.Services.AddTransientWithShellRoute<CreateAdvertisment, CreateAdvertismentViewModel>($"{nameof(CreateAdvertisment)}");

        builder.Services.AddScoped<AdToLocation>();

        builder.Services.AddScoped<SearchAd>();
        
        builder.Services.AddScoped<MyContentPage>();

        builder.Services.AddScoped<Login>();

        builder.Services.AddScoped<Register>();

        builder.Services.AddScoped<GoogleMapPage>();

        builder.Services.AddScoped<UserSetting>();

        builder.Services.AddTransientWithShellRoute<EditAdvertisment, EditAdvertismentViewModel>($"{nameof(EditAdvertisment)}");

        builder.Services.AddScoped<EditUserPerfil>();

        builder.Services.AddScoped<EditLoginCredential>();

        builder.Services.AddScoped<RecoveryPasswordPopUp>();

        builder.Services.AddScoped<MyFavorites>();

        //builder.Services.AddSingleton<ManageLocation>();

        builder.Services.AddTransientWithShellRoute<ManageLocation, ManageLocationViewModel>($"{nameof(ManageLocation)}");

        #endregion Pages

        #region Tools

        builder.Services.AddSingleton<LogUserPerfilTool>();

        #endregion Tools

        return builder.Build();
    }
}