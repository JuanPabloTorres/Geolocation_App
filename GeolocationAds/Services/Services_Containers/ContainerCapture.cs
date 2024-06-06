using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class ContainerCapture : BaseContainer, IContainerCapture
    {
        public ContainerCapture(Capture model, LogUserPerfilTool logUserPerfilTool, IAdvertisementService advertisementService, ICaptureService captureService, IAppSettingService appSettingService) : base(logUserPerfilTool, appSettingService)
        {
            this.AdvertisementService = advertisementService;

            this.CaptureService = captureService;

            this.Model = model;
        }

        public IAdvertisementService AdvertisementService { get; }

        public Capture Model { get; }

        public ICaptureService CaptureService { get; }
    }
}
