using ToolsLibrary.Models;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerEditAdvertisment : IBaseContainer
    {
        IAdvertisementService AdvertisementService { get; }

        Advertisement Model { get; }
    }
}