using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class Register : ContentPage
{
    RegisterViewModel _registerViewModel;

    public Register(RegisterViewModel registerViewModel)
    {
        InitializeComponent();

        this._registerViewModel = registerViewModel;

        BindingContext = registerViewModel;
    }

    protected override void OnAppearing()
    {
        this._registerViewModel.ValidationResults.Clear();
    }
}