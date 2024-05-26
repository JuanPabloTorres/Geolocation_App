using ToolsLibrary.Models;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerEditAdvertisment : IBaseContainer
    {
        IAppSettingService AppSettingService { get; }

        IAdvertisementService AdvertisementService { get; }

        Advertisement Model { get; }
    }
}
