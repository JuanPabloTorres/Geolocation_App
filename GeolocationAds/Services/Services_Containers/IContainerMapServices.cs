using Microsoft.Maui.Controls.Maps;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerMapServices : IBaseContainer
    {
        IAppSettingService AppSettingService { get; }
        IGeolocationAdService GeolocationAdService { get; }
        Pin Model { get; }
    }
}
