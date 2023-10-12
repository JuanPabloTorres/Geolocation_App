using GeolocationAds.ViewModels;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class MyFavorites : ContentPage
{
    private CaptureViewModel viewModel;

    public MyFavorites(CaptureViewModel captureViewModel)
    {
        InitializeComponent();

        viewModel = captureViewModel;

        this.BindingContext = captureViewModel;
    }

    private async void NextItemButton_Clicked(object sender, EventArgs e)
    {
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
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
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
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
    }
}