using CommunityToolkit.Maui.Views;
using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    private CreateAdvertismentViewModel viewModel;

    public CreateAdvertisment(CreateAdvertismentViewModel createGeolocationViewModel)
    {
        InitializeComponent();

        viewModel = createGeolocationViewModel;

        BindingContext = createGeolocationViewModel;
    }

    protected override async void OnAppearing()
    {
        await viewModel.LoadSetting();
    }

    private void MediaElement_MediaFailed(object sender, CommunityToolkit.Maui.Core.Primitives.MediaFailedEventArgs e)
    {
    }

    private void OnMediaEnded(object sender, EventArgs e)
    {
    }

    private void OnMediaOpened(object sender, EventArgs e)
    {
    }

    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
    }

    private void MediaElement_StateChanged(object sender, CommunityToolkit.Maui.Core.Primitives.MediaStateChangedEventArgs e)
    {
        var _md = sender as MediaElement;

        if (_md.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
        {
            this.ScrollView01.IsEnabled = false;
        }
        else
        {
            this.ScrollView01.IsEnabled = true;
        }
    }
}