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

    //private void NextItemButton_Clicked(object sender, EventArgs e)
    //{
    //    //if (carouselView.Position < (carouselView.ItemsSource?.Count - 1))
    //    //{
    //    //    carouselView.Position++;
    //    //}

    //    //carouselView.IsSwipeEnabled = true;

    //    if (carouselView.ItemsSource != null && carouselView.Position < (carouselView.ItemsSource.Cast<object>().ToList().Count - 1))
    //    {
    //        carouselView.Position++;
    //    }

    //    //carouselView.IsSwipeEnabled = false;

    //}

    //private void BackItemButton_Clicked(object sender, EventArgs e)
    //{
    //    if (carouselView.Position > 0)
    //    {
    //        carouselView.Position--;
    //    }
    //}

    private async void NextItemButton_Clicked(object sender, EventArgs e)
    {
        //if (carouselView.Position < (carouselView.ItemsSource?.Count - 1))
        //{
        //    carouselView.Position++;
        //}

        //carouselView.IsSwipeEnabled = true;
        try
        {
            if (carouselView.ItemsSource != null && carouselView.Position < (carouselView.ItemsSource.Cast<object>().ToList().Count - 1))
            {
                carouselView.Position++;
            }
            if (carouselView.Position == carouselView.ItemsSource.Cast<object>().ToList().Count - 1)
            {
                this.NextBtn.IsEnabled = false;

                this.BackBtn.IsEnabled = true;
            }
            else
            {
                this.NextBtn.IsEnabled = true;

                this.BackBtn.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        //carouselView.IsSwipeEnabled = false;
    }

    private async void BackItemButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (carouselView.Position > 0)
            {
                carouselView.Position--;
            }

            if (carouselView.Position == 0)
            {
                this.BackBtn.IsEnabled = false;

                this.NextBtn.IsEnabled = true;
            }
            else
            {
                this.BackBtn.IsEnabled = true;

                this.NextBtn.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}