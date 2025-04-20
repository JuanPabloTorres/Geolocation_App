using Microsoft.Maui.Controls.Maps;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class ContainerMapServices : BaseContainer, IContainerMapServices
    {
        public ContainerMapServices(LogUserPerfilTool logUserPerfilTool, Pin model, IGeolocationAdService geolocationAdService, IRestrictedZoneService restrictedZoneService, IAppSettingService appSettingService) : base(logUserPerfilTool, appSettingService)
        {
            GeolocationAdService = geolocationAdService;

            RestrictedZoneService = restrictedZoneService;

            Model = model;
        }

        public IGeolocationAdService GeolocationAdService { get; }

        public Pin Model { get; }

        public IRestrictedZoneService RestrictedZoneService { get; }
    }
}