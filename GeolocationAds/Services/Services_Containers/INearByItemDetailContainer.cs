using ToolsLibrary.Models;
namespace GeolocationAds.Services.Services_Containers
{
    public interface INearByItemDetailContainer : IBaseContainer
    {


        IAdvertisementService AdvertisementService { get; }

        Advertisement Model { get; }
    }
}
