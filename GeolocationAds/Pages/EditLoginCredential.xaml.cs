using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditLoginCredential : ContentPage
{
    private EditLoginCredentialViewModel _editLoginCredentialViewModel;

    public EditLoginCredential(EditLoginCredentialViewModel editLoginCredentialViewModel)
    {
        InitializeComponent();

        this._editLoginCredentialViewModel = editLoginCredentialViewModel;

        this.BindingContext = editLoginCredentialViewModel;
    }

    protected override void OnAppearing()
    {
        this._editLoginCredentialViewModel.ValidationResults.Clear();
    }
}