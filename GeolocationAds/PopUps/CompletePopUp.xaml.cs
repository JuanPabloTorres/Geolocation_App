using CommunityToolkit.Maui.Views;

namespace GeolocationAds.PopUps;

public partial class CompletePopUp : Popup
{
    public CompletePopUp()
    {
        InitializeComponent();
    }

    private async void SKLottieView_AnimationCompleted(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}