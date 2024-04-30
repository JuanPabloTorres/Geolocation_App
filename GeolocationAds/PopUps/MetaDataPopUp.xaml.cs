using CommunityToolkit.Maui.Views;
using GeolocationAds.ViewModels;
using ToolsLibrary.Models;

namespace GeolocationAds.PopUps;

public partial class MetaDataPopUp : Popup
{
    public MetaDataPopUp(MetaDataViewModel vm)
    {
        InitializeComponent();

        this.BindingContext = vm;
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}