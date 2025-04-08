using CommunityToolkit.Maui.Views;

namespace GeolocationAds.PopUps;

public partial class CapturePopUp : Popup
{
	public CapturePopUp()
	{
		InitializeComponent();
	}

    private void SKLottieView_AnimationCompleted(object sender, EventArgs e)
    {
        this.Close(); // Cierra el popup automáticamente al completar
    }
}