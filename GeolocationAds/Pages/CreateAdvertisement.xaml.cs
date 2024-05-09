using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using ToolsLibrary.Models;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    private CreateAdvertismentViewModel2 viewModel;

    public CreateAdvertisment(CreateAdvertismentViewModel2 createGeolocationViewModel)
    {
        InitializeComponent();

        viewModel = createGeolocationViewModel;

        BindingContext = createGeolocationViewModel;

        contentCollection.Loaded += ContentCollection_Loaded; ;

        contentCollection.Scrolled += Handle_Scrolled;
    }

    private async void ContentCollection_Loaded(object sender, EventArgs e)
    {

        var _template = (CarouselView)sender;

        //_template.ItemsSource.Cast<ContentTypeTemplateViewModel2>().ToList();

        //if (_template.ContentVisualType == ContentVisualType.Image)
        //{
        //    await _template.SetAnimation();  // Call SetAnimation only for visible items
        //}

        foreach (var item in _template.ItemsSource.Cast<ContentTypeTemplateViewModel2>().ToList())
        {
            if (item.ContentVisualType == ContentVisualType.Image)
            {
                await item.SetAnimation();  // Call SetAnimation only for visible items
            }

        }
    }

    protected override async void OnAppearing()
    {
        if (this.viewModel.AdTypesSettings.Count() == 0)
        {
            await this.viewModel.InitializeSettings();

            this.viewModel.SetDefault();
        }
        else
        {
            this.viewModel.SetDefault();
        }
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
            //viewModel.ContentTypesTemplate[i].SetAnimation() = i >= start && i <= end;
        }
    }
}