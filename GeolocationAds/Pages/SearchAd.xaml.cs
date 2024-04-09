using GeolocationAds.ViewModels;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class SearchAd : ContentPage
{
    private SearchAdViewModel viewModel;

    public SearchAd(SearchAdViewModel searchAdViewModel)
    {
        InitializeComponent();

        this.viewModel = searchAdViewModel;

        BindingContext = searchAdViewModel;
    }

    protected override void OnAppearing()
    {
        this.viewModel.NearByTemplateViewModels.Clear();
    }

    private async void BackItemButton_Clicked(object sender, EventArgs e)
    {
        await ChangePositionBy(-1);
    }

    private async void NextItemButton_Clicked(object sender, EventArgs e)
    {
        await ChangePositionBy(1);
    }

    private async Task ChangePositionBy(int delta)
    {
        try
        {
            viewModel.IsLoading = true;

            // Cache the item count to avoid multiple enumerations
            var itemCount = carouselView.ItemsSource?.Cast<object>().Count() ?? 0;

            if (itemCount == 0) return; // Exit if there are no items

            var newPosition = carouselView.Position + delta;

            newPosition = Math.Max(0, Math.Min(newPosition, itemCount - 1)); // Clamp to valid range

            carouselView.Position = newPosition;

            UpdateButtonStates(newPosition, itemCount);
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

    private void UpdateButtonStates(int currentPosition, int itemCount)
    {
        this.BackBtn.IsEnabled = currentPosition > 0;

        this.NextBtn.IsEnabled = currentPosition < itemCount - 1;
    }
}