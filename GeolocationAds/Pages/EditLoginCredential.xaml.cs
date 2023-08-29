using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditLoginCredential : ContentPage
{
    public EditLoginCredential(EditLoginCredentialViewModel editLoginCredentialViewModel)
    {
        InitializeComponent();

        BindingContext = editLoginCredentialViewModel;
    }
}