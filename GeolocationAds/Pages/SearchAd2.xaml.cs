using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class SearchAd2 : ContentPage
{
    public SearchAd2(SearchAdViewModel searchAdViewModel)
    {
        InitializeComponent();

        BindingContext = searchAdViewModel;
    }
}