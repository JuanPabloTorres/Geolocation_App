using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Maps;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ToolsLibrary.Models;
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

        var configuration = CommonsTool.ConfigurationLoader.LoadConfiguration();

        builder.Services.AddSingleton<IConfiguration>(configuration);

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
                Timeout = TimeSpan.FromMinutes(ConstantsTools.TIMEOUT)
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

        #endregion ViewModels

        #region Containers

        builder.Services.AddScoped<IContainerMyContentServices, ContainerMyContentServices>();

        builder.Services.AddScoped<IContainerMapServices, ContainerMapServices>();

        builder.Services.AddScoped<IContainerEditAdvertisment, ContainerEditAdvertisment>();

        builder.Services.AddScoped<IContainerCreateAdvertisment, ContainerCreateAdvertisment>();

        builder.Services.AddScoped<INearByItemDetailContainer, NearByItemDetailContainer>();

        builder.Services.AddScoped<IContainerManageLocation, ContainerManageLocation>();

        builder.Services.AddScoped<IContainerCapture, ContainerCapture>();

        #endregion Containers

        #region Pages

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

        builder.Services.AddTransientWithShellRoute<ManageLocation, ManageLocationViewModel2>($"{nameof(ManageLocation)}");

        #endregion Pages

        #region Tools

        builder.Services.AddSingleton<LogUserPerfilTool>();

        #endregion Tools

        return builder.Build();
    }
}