using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class UserSetting : ContentPage
{
    public UserSetting(UserSettingViewModel userSettingViewModel)
    {
        InitializeComponent();

        BindingContext = userSettingViewModel;
    }
}