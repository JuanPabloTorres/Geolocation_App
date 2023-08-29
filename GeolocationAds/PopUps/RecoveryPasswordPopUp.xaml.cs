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
}