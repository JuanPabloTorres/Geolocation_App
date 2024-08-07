

using CommunityToolkit.Maui.Views;
using GeolocationAds.ViewModels;

namespace GeolocationAds.PopUps;

public partial class FilterPopUpForSearch : Popup
{
    public FilterPopUpForSearch(FilterPopUpViewModel2 filterPopUpViewModel)
    {
        InitializeComponent();

        BindingContext = filterPopUpViewModel;
    }

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