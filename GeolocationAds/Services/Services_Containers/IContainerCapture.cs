using ToolsLibrary.Models;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerCapture : IBaseContainer
    {
        IAdvertisementService AdvertisementService { get; }

        ICaptureService CaptureService { get; }

        Capture Model { get; }
    }
}
