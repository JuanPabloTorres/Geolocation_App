// Inside your Platforms/Android folder
using Android.Content.Res;
using GeolocationAds.Platforms.Android.Services;
using GeolocationAds.PlatformService;
using Microsoft.Maui.Handlers;
using Color = Android.Graphics.Color;

// Make sure to register the dependency
[assembly: Dependency(typeof(GeolocationAds.Platforms.Android.Services.AndroidPlatformServices))]
namespace GeolocationAds.Platforms.Android.Services
{
    public class AndroidPlatformServices : IPlatformServices
    {
        public AndroidPlatformServices()
        {
        }
        public void SetEntryBackgroundColor(Microsoft.Maui.Controls.Entry entry)
        {
            var handler = entry.Handler as EntryHandler;

            if (handler?.PlatformView != null)
            {
                handler.PlatformView.SetBackgroundColor(Color.Transparent);

                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
            }
        }
    }
}