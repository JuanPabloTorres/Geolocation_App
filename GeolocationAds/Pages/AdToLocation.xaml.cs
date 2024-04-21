using GeolocationAds.ViewModels;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class AdToLocation : ContentPage
{
    private MyContentViewModel viewModel;

    public AdToLocation(MyContentViewModel adToLocationViewModel)
    {
        InitializeComponent();

        this.viewModel = adToLocationViewModel;

        BindingContext = adToLocationViewModel;
    }

    protected override async void OnAppearing()
    {
        MyContentViewModel.PageIndex = 1;

        this.viewModel.CollectionModel.Clear();

        await this.viewModel.InitializeSettingsAsync();

        await this.viewModel.InitializeAsync();
    }

    private async void NextItemButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            viewModel.IsLoading = true;

            this.NextBtn.IsEnabled = false;

            this.BackBtn.IsEnabled = false;

            if (carouselView.ItemsSource != null && carouselView.Position < (carouselView.ItemsSource.Cast<object>().ToList().Count - 1))
            {
                carouselView.Position++;
            }
            if (carouselView.Position == carouselView.ItemsSource.Cast<object>().ToList().Count - 1)
            {
                MyContentViewModel.PageIndex++;

                await this.viewModel.InitializeAsync(MyContentViewModel.PageIndex);

                //this.NextBtn.IsEnabled = false;

                //this.BackBtn.IsEnabled = true;
            }
            else
            {
                //this.NextBtn.IsEnabled = true;

                //this.BackBtn.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
        finally
        {
            viewModel.IsLoading = false;

            this.NextBtn.IsEnabled = true;

            this.BackBtn.IsEnabled = true;
        }
    }

    private async void BackItemButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            viewModel.IsLoading = true;

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
        finally
        {
            viewModel.IsLoading = false;
        }
    }


}