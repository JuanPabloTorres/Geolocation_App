using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class SearchAd : ContentPage
{
    private SearchAdViewModel2 viewModel;

    public SearchAd(SearchAdViewModel2 searchAdViewModel)
    {
        InitializeComponent();

        this.viewModel = searchAdViewModel;

        BindingContext = searchAdViewModel;

        this.paginControls.NextClicked += NextItemButton_Clicked;

        this.paginControls.BackClicked += BackItemButton_Clicked;

        this.viewModel.NearByTemplateViewModels.CollectionChanged += NearByTemplateViewModels_CollectionChanged;
    }

    private void NearByTemplateViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        var collection = sender as ObservableCollection<NearByTemplateViewModel2>;

        if (!collection.IsObjectNull())
        {
            this.counterLabel.IsVisible = collection.Count > 0;

            this.filterType.IsVisible = collection.Count > 0;

            this.filterDistance.IsVisible = collection.Count > 0;
        }
    }

    protected override void OnAppearing()
    {
        if (this.viewModel.NearByTemplateViewModels.Count == 0)
        {
            SearchAdViewModel2.PageIndex = 1;

            this.counterLabel.IsVisible = false;

            this.filterType.IsVisible = false;

            this.filterDistance.IsVisible = false;

            this.paginControls.IsBackButtonEnabled = false;

            this.paginControls.IsNextButtonEnabled = true;

            this.paginControls.IsNextButtonVisible = false;

            this.paginControls.IsBackButtonVisible = false;
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

    private async void NextItemButton_Clicked(object sender, EventArgs e)
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

                SearchAdViewModel2.PageIndex++;

                await this.viewModel.InitializeDataLoadingAsync(SearchAdViewModel2.PageIndex);

                var _newCount = GetSourceLastIndexCount();

                if (_newCount > _oldCount)
                {
                    carouselView.Position++;
                }

                if (_newCount == _oldCount)
                {
                    this.paginControls.IsBackButtonEnabled = true;

                    SearchAdViewModel2.PageIndex--;
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

    private async Task ChangePositionBy(int delta)
    {
        try
        {
            viewModel.IsLoading = true;

            // Cache the item count to avoid multiple enumerations
            var itemCount = GetSourceLastIndexCount();

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
        this.paginControls.IsBackButtonEnabled = currentPosition > 0;

        this.paginControls.IsNextButtonEnabled = currentPosition < itemCount - 1;
    }
}