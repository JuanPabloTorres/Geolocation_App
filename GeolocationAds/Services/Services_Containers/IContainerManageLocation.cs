using ToolsLibrary.Models;


namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerManageLocation : IBaseContainer
    {
        IGeolocationAdService GeolocationAdService { get; }

        IAdvertisementService AdvertisementService { get; }

        Advertisement Model { get; }
    }
}
