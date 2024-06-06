using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class NearByItemDetailContainer : BaseContainer, INearByItemDetailContainer
    {
        public NearByItemDetailContainer(Advertisement model, IAdvertisementService advertisementService, IAppSettingService appSettingService, LogUserPerfilTool logUserPerfilTool) : base(logUserPerfilTool, appSettingService)
        {


            this.AdvertisementService = advertisementService;

            this.Model = model;
        }



        public IAdvertisementService AdvertisementService { get; }

        public Advertisement Model { get; }
    }
}