using CommunityToolkit.Maui.Views;
using GeolocationAds.ViewModels;

namespace GeolocationAds.PopUps;

public partial class FilterPopUp : Popup
{
    public FilterPopUp(FilterPopUpViewModel2 filterPopUpViewModel)
    {
        InitializeComponent();

        BindingContext = filterPopUpViewModel;
    }

    public FilterPopUp(FilterPopUpViewModel filterPopUpViewModel)
    {
        InitializeComponent();

        BindingContext = filterPopUpViewModel;
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}