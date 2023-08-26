using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditUserPerfil : ContentPage
{
    public EditUserPerfil(EditUserPerfilViewModel editUserPerfilViewModel)
    {
        InitializeComponent();

        BindingContext = editUserPerfilViewModel;
    }
}