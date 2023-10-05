using CommunityToolkit.Maui.Views;
using GeolocationAds.ViewModels;

namespace GeolocationAds.PopUps;

public partial class FilterPopUpForSearch : Popup
{
    public FilterPopUpForSearch(FilterPopUpViewModel filterPopUpViewModel)
    {
        InitializeComponent();

        BindingContext = filterPopUpViewModel;
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}