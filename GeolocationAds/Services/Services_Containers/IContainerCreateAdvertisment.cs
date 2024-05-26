using ToolsLibrary.Models;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerCreateAdvertisment : IBaseContainer
    {
        IAppSettingService AppSettingService { get; }

        IAdvertisementService AdvertisementService { get; }

        Advertisement Model { get; }
    }
}
