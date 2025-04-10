using CommunityToolkit.Maui.Views;

namespace GeolocationAds.PopUps;

public partial class NotFoundPopup : Popup
{
	public NotFoundPopup(string notFoundText)
	{
		InitializeComponent();

        BindingContext = notFoundText;
	}

    public NotFoundPopup()
    {
        InitializeComponent();
    }

    private void SKLottieView_AnimationCompleted(object sender, EventArgs e)
    {
        this.Close(); // Cierra el popup automáticamente al completar
    }
}