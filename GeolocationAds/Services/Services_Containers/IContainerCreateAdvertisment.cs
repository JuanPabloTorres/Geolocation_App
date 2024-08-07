using ToolsLibrary.Models;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerCreateAdvertisment : IBaseContainer
    {
        IAdvertisementService AdvertisementService { get; }

        Advertisement Model { get; }
    }
}