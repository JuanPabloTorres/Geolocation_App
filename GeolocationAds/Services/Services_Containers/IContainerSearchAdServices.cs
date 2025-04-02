using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerSearchAdServices:IBaseContainer
    {
        ICaptureService CaptureService { get; }
        IAdvertisementService AdvertisementService { get; }
        IGeolocationAdService GeolocationAdService { get; }
        IAppSettingService AppSettingService { get; }

        Advertisement Advertisement { get; }
    }
}
