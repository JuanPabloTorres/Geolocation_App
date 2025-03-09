using GeolocationAds.AppTools;
using GeolocationAds.CustomContentView;
using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class Register : ContentPage
{
    private RegisterViewModel _registerViewModel;

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

    private void onPhoneChange(object sender, TextChangedEventArgs e)
    {
        if (sender is CustomEntryContentView entryView && entryView.BindingContext is RegisterViewModel viewModel)
        {
            viewModel.Model.Phone = AppToolCommon.FormatPhoneNumber(e.NewTextValue);
        }
    }

    private void onEmailChange(object sender, TextChangedEventArgs e)
    {
        if (sender is CustomEntryContentView entryView && entryView.BindingContext is RegisterViewModel viewModel)
        {
            string formattedEmail = FormatEmail(e.NewTextValue);

            viewModel.Model.Email = formattedEmail;
        }
    }

    private string FormatEmail(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        input = input.Trim(); // Remover espacios en blanco al inicio y al final

        // Validar caracteres no permitidos
        if (input.Any(c => c == ' '))
            return input.Replace(" ", ""); // Elimina espacios en blanco

        return input;
    }
}