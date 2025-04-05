using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditUserPerfil : ContentPage
{
    public EditUserPerfil(EditUserPerfilViewModel editUserPerfilViewModel)
    {
        InitializeComponent();

        BindingContext = editUserPerfilViewModel;
    }

    protected override void OnAppearing()
    {
        if (BindingContext is EditUserPerfilViewModel viewModel)
        {
            //viewModel.UpdateModel();

            viewModel.ValidationResults.Clear();
        }
    }
}