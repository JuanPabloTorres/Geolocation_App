using GeolocationAds.TemplateViewModel;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerMyContentServices : IBaseContainer
    {
        ContentViewTemplateViewModel AdLocationTemplateViewModel { get; }
        IAdvertisementService AdvertisementService { get; }
        IGeolocationAdService GeolocationAdService { get; }


    }
}
