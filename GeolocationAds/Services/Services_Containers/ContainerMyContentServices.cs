using GeolocationAds.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class ContainerMyContentServices : BaseContainer, IContainerMyContentServices
    {
        public ContainerMyContentServices(ContentViewTemplateViewModel adLocationTemplateViewModel, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUserPerfilTool) : base(logUserPerfilTool)
        {
            AdLocationTemplateViewModel = adLocationTemplateViewModel;
            AdvertisementService = advertisementService;
            GeolocationAdService = geolocationAdService;
            AppSettingService = appSettingService;
        }

        public ContentViewTemplateViewModel AdLocationTemplateViewModel { get; }
        public IAdvertisementService AdvertisementService { get; }
        public IAppSettingService AppSettingService { get; }
        public IGeolocationAdService GeolocationAdService { get; }
    }
}