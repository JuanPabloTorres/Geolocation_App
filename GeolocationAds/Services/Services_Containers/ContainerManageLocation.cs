using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class ContainerManageLocation : BaseContainer, IContainerManageLocation
    {
        public ContainerManageLocation(Advertisement model, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, LogUserPerfilTool logUserPerfilTool) : base(logUserPerfilTool)
        {
            this.GeolocationAdService = geolocationAdService;

            this.AdvertisementService = advertisementService;

            this.Model = model;
        }

        public IGeolocationAdService GeolocationAdService { get; }

        public IAdvertisementService AdvertisementService { get; }

        public Advertisement Model { get; }
    }
}
