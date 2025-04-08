using CommunityToolkit.Maui.Views;

namespace GeolocationAds.PopUps;

public partial class RemovePopUp : Popup
{
	public RemovePopUp()
	{
		InitializeComponent();
	}

    private async void SKLottieView_AnimationCompleted(object sender, EventArgs e)
    {
        // Espera breve antes de cerrar el popup
        //await Task.Delay(300);

        Close();
    }
}