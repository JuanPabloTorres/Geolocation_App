using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
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

        this.paginControls.OnNextClickedAction = NextItemButton_Clicked;

        this.paginControls.OnBackClickedAction = BackItemButton_Clicked;

        this.viewModel.NearByTemplateViewModels.CollectionChanged += NearByTemplateViewModels_CollectionChanged;
    }

    private void NearByTemplateViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        var collection = sender as ObservableCollection<NearByTemplateViewModel2>;

        if (!collection.IsObjectNull())
        {
            statusBar.IsVisible = collection.Count > 0;
        }
    }

    

    protected async override void OnAppearing()
    {
        if (this.viewModel.NearByTemplateViewModels.Count == 0)
        {
            SearchAdViewModel.PageIndex = 1;

            statusBar.IsVisible = false;

            this.paginControls.IsBackButtonEnabled = false;

            this.paginControls.IsNextButtonEnabled = true;

            this.paginControls.IsNextButtonVisible = false;

            this.paginControls.IsBackButtonVisible = false;

           
        }

        //if (viewModel.SelectedAdType.IsObjectNull())
        //{
        //    await viewModel.InitializeDataLoadingSettingsAsync();
        //}
    }

    private async void BackItemButton_Clicked()
    {
        await viewModel.RunWithLoadingIndicator(async () =>
        {
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
        });
    }

    private async void NextItemButton_Clicked()
    {
        await this.viewModel.RunWithLoadingIndicator(async () =>
        {
            this.paginControls.IsNextButtonEnabled = false;

            this.paginControls.IsBackButtonEnabled = false;

            if (carouselView.ItemsSource != null && carouselView.Position < GetSourceLastIndexCount())
            {
                carouselView.Position++;
            }

            if (carouselView.Position == GetSourceLastIndexCount())
            {
                var _oldCount = GetSourceLastIndexCount();

                SearchAdViewModel.PageIndex++;

                await this.viewModel.InitializeDataLoadingAsync(SearchAdViewModel.PageIndex);

                var _newCount = GetSourceLastIndexCount();

                if (_newCount > _oldCount)
                {
                    carouselView.Position++;
                }

                if (_newCount == _oldCount)
                {
                    this.paginControls.IsBackButtonEnabled = true;
                    SearchAdViewModel.PageIndex--;
                }
            }

            this.paginControls.IsNextButtonEnabled = true;

            this.paginControls.IsBackButtonEnabled = true;
        });
    }

    private int GetSourceLastIndexCount()
    {
        return carouselView.ItemsSource.Cast<object>().ToList().Count - 1;
    }

    private void UpdateButtonStates(int currentPosition, int itemCount)
    {
        this.paginControls.IsBackButtonEnabled = currentPosition > 0;

        this.paginControls.IsNextButtonEnabled = currentPosition < itemCount - 1;
    }
}