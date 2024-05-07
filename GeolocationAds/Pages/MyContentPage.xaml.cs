using GeolocationAds.ViewModels;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class MyContentPage : ContentPage
{
    private MyContentViewModel2 _viewModel;

    public MyContentPage(MyContentViewModel2 vm)
    {
        InitializeComponent();

        this._viewModel = vm;

        BindingContext = vm;

        this.paginControls.NextClicked += NextItemButton_Clicked;

        this.paginControls.BackClicked += BackItemButton_Clicked;
    }

    protected override async void OnAppearing()
    {
        MyContentViewModel2.PageIndex = 1;

        this._viewModel.CollectionModel.Clear();

        this.paginControls.IsBackButtonEnabled = false;

        this.paginControls.IsNextButtonEnabled = true;

        await this._viewModel.InitializeSettingsAsync();

        await this._viewModel.InitializeAsync();
    }

    private int GetSourceLastIndexCount()
    {
        return carouselView.ItemsSource.Cast<object>().ToList().Count - 1;
    }

    private async void NextItemButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            _viewModel.IsLoading = true;

            this.paginControls.IsNextButtonEnabled = false;

            this.paginControls.IsBackButtonEnabled = false;

            if (carouselView.ItemsSource != null && carouselView.Position < GetSourceLastIndexCount())
            {
                carouselView.Position++;
            }

            if (carouselView.Position == GetSourceLastIndexCount())
            {
                var _oldCount = GetSourceLastIndexCount();

                MyContentViewModel2.PageIndex++;

                await this._viewModel.InitializeAsync(MyContentViewModel2.PageIndex);

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
            _viewModel.IsLoading = false;

            this.paginControls.IsNextButtonEnabled = true;

            this.paginControls.IsBackButtonEnabled = true;
        }
    }

    private async void BackItemButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            _viewModel.IsLoading = true;

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
            _viewModel.IsLoading = false;
        }
    }
}