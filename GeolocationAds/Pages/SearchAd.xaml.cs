using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class SearchAd : ContentPage
{
    public SearchAd(SearchAdViewModel searchAdViewModel)
    {
        InitializeComponent();

        BindingContext = searchAdViewModel;
    }
}