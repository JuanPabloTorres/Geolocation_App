using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class ContainerCreateAdvertisment : BaseContainer, IContainerCreateAdvertisment
    {
        public ContainerCreateAdvertisment(Advertisement model, IAdvertisementService advertisementService, IAppSettingService appSettingService, LogUserPerfilTool logUserPerfilTool) : base(logUserPerfilTool)
        {
            this.AppSettingService = appSettingService;

            this.AdvertisementService = advertisementService;

            this.Model = model;
        }

        public IAppSettingService AppSettingService { get; }

        public IAdvertisementService AdvertisementService { get; }

        public Advertisement Model { get; }
    }
}
