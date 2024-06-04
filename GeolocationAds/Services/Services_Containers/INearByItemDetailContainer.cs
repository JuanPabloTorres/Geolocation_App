using ToolsLibrary.Models;
namespace GeolocationAds.Services.Services_Containers
{
    public interface INearByItemDetailContainer : IBaseContainer
    {
        IAppSettingService AppSettingService { get; }

        IAdvertisementService AdvertisementService { get; }

        Advertisement Model { get; }
    }
}
