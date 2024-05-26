using Microsoft.Maui.Controls.Maps;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class ContainerMapServices : BaseContainer, IContainerMapServices
    {
        public ContainerMapServices(LogUserPerfilTool logUserPerfilTool, Pin model, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService) : base(logUserPerfilTool)
        {
            GeolocationAdService = geolocationAdService;

            AppSettingService = appSettingService;

            Model = model;
        }

        public IAppSettingService AppSettingService { get; }
        public IGeolocationAdService GeolocationAdService { get; }

        public Pin Model { get; }

    }
}
