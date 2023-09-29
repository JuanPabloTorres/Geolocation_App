using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class AdToLocation : ContentPage
{
    private AdToLocationViewModel viewModel;

    public AdToLocation(AdToLocationViewModel adToLocationViewModel)
    {
        InitializeComponent();

        this.viewModel = adToLocationViewModel;

        BindingContext = adToLocationViewModel;
    }

    protected override void OnAppearing()
    {
        viewModel.Initialize();
    }

    private void NextItemButton_Clicked(object sender, EventArgs e)
    {
        //if (carouselView.Position < (carouselView.ItemsSource?.Count - 1))
        //{
        //    carouselView.Position++;
        //}

        //carouselView.IsSwipeEnabled = true;

        if (carouselView.ItemsSource != null && carouselView.Position < (carouselView.ItemsSource.Cast<object>().ToList().Count - 1))
        {
            carouselView.Position++;
        }

        //carouselView.IsSwipeEnabled = false;

    }

    private void BackItemButton_Clicked(object sender, EventArgs e)
    {
        if (carouselView.Position > 0)
        {
            carouselView.Position--;
        }
    }
}