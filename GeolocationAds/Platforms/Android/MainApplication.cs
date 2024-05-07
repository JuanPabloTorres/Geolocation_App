using Android.App;
using Android.Runtime;

namespace GeolocationAds;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        });
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}