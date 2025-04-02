using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class ContainerSearchAdServices : BaseContainer, IContainerSearchAdServices
    {
        public ContainerSearchAdServices(
            ICaptureService captureService,
            IAdvertisementService advertisementService,
            IGeolocationAdService geolocationAdService,
            IAppSettingService appSettingService,
            Advertisement advertisement,
            LogUserPerfilTool logUserPerfilTool)
            : base(logUserPerfilTool, appSettingService)
        {
            CaptureService = captureService;

            AdvertisementService = advertisementService;

            GeolocationAdService = geolocationAdService;

            AppSettingService = appSettingService;

            Advertisement = advertisement;
        }

        public ICaptureService CaptureService { get; }

        public IAdvertisementService AdvertisementService { get; }

        public IGeolocationAdService GeolocationAdService { get; }

        public IAppSettingService AppSettingService { get; }

        public Advertisement Advertisement { get; }
    }
}