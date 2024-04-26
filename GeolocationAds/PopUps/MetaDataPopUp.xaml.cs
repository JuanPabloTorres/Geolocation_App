using CommunityToolkit.Maui.Views;
using ToolsLibrary.Models;

namespace GeolocationAds.PopUps;

public partial class MetaDataPopUp : Popup
{
    public MetaDataPopUp(Advertisement advertisement)
    {
        InitializeComponent();

        this.BindingContext = advertisement;
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }
}