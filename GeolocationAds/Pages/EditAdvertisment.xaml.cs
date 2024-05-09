using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using ToolsLibrary.Models;

namespace GeolocationAds.Pages;

public partial class EditAdvertisment : ContentPage
{
    private EditAdvertismentViewModel2 viewModel;

    public EditAdvertisment(EditAdvertismentViewModel2 editAdvertismentViewModel)
    {
        InitializeComponent();

        this.viewModel = editAdvertismentViewModel;

        BindingContext = editAdvertismentViewModel;

        contentCollection.Loaded += ContentCollection_Loaded; ;

        contentCollection.Scrolled += Handle_Scrolled;
    }

    private async void Handle_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        int start = e.FirstVisibleItemIndex;

        int end = e.LastVisibleItemIndex;

        for (int i = 0; i < viewModel.ContentTypesTemplate.Count; i++)
        {
            var item = viewModel.ContentTypesTemplate[i];

            if (item.ContentVisualType == ContentVisualType.Image)
            {
                if (i >= start && i <= end)
                {
                    await item.SetAnimation();  // Call SetAnimation only for visible items
                }
            }
        }
    }

    private async void ContentCollection_Loaded(object sender, EventArgs e)
    {
        var _template = (CarouselView)sender;

        foreach (var item in _template.ItemsSource.Cast<ContentTypeTemplateViewModel2>().ToList())
        {
            if (item.ContentVisualType == ContentVisualType.Image)
            {
                await item.SetAnimation();  // Call SetAnimation only for visible items
            }
        }
    }
}