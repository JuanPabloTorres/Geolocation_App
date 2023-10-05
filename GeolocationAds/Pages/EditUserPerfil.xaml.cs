using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditUserPerfil : ContentPage
{
    public EditUserPerfilViewModel _editUserPerfilViewModel;

    public EditUserPerfil(EditUserPerfilViewModel editUserPerfilViewModel)
    {
        InitializeComponent();

        this._editUserPerfilViewModel = editUserPerfilViewModel;

        BindingContext = editUserPerfilViewModel;
    }

    protected override void OnAppearing()
    {
        this._editUserPerfilViewModel.ValidationResults.Clear();
    }
}