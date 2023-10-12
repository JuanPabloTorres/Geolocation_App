using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class ManageLocation : ContentPage
{
    public ManageLocation(ManageLocationViewModel manageLocationViewModel)
    {
        InitializeComponent();

        this.BindingContext = manageLocationViewModel;
    }
}