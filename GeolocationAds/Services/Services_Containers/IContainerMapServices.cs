using Microsoft.Maui.Controls.Maps;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IContainerMapServices : IBaseContainer
    {

        IGeolocationAdService GeolocationAdService { get; }
        
        IRestrictedZoneService RestrictedZoneService { get; }
        Pin Model { get; }
    }
}
