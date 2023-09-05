using CommunityToolkit.Maui.Views;
using GeolocationAds.ViewModels;

namespace GeolocationAds.PopUps;

public partial class RecoveryPasswordPopUp : Popup
{
    public RecoveryPasswordPopUp(RecoveryPasswordViewModel recoveryPasswordViewModel)
    {
        InitializeComponent();

        BindingContext = recoveryPasswordViewModel;
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}