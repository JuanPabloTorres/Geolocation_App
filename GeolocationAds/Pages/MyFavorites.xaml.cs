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

        this.paginControls.OnNextClickedAction = NextItemButton_Clicked;

        this.paginControls.OnBackClickedAction = BackItemButton_Clicked;
    }

    private async void BackItemButton_Clicked()
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
                this.paginControls.IsBackButtonEnabled = false;

                this.paginControls.IsNextButtonEnabled = true;
            }
            else
            {
                this.paginControls.IsBackButtonEnabled = true;

                this.paginControls.IsNextButtonEnabled = true;
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

    private async void NextItemButton_Clicked()
    {
        try
        {
            this.viewModel.IsLoading = true;

            this.paginControls.IsNextButtonEnabled = false;

            this.paginControls.IsBackButtonEnabled = false;

            if (carouselView.ItemsSource != null && carouselView.Position < GetSourceLastIndexCount())
            {
                carouselView.Position++;
            }

            if (carouselView.Position == GetSourceLastIndexCount())
            {
                var _oldCount = GetSourceLastIndexCount();

                CaptureViewModel.PageIndex++;

                await this.viewModel.InitializeAsync(CaptureViewModel.PageIndex);

                var _newCount = GetSourceLastIndexCount();

                if (_newCount > _oldCount)
                {
                    carouselView.Position++;
                }

                if (_newCount == _oldCount)
                {
                    this.paginControls.IsBackButtonEnabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
        finally
        {
            this.viewModel.IsLoading = false;

            this.paginControls.IsNextButtonEnabled = true;

            this.paginControls.IsBackButtonEnabled = true;
        }
    }

    private int GetSourceLastIndexCount()
    {
        return carouselView.ItemsSource.Cast<object>().ToList().Count - 1;
    }
}