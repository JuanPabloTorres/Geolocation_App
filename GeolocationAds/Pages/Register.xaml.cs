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

    //private void onPhoneChange(object sender, TextChangedEventArgs e
    //{
    //    if (sender is CustomEntryContentView entryView            && entryView.BindingContext is RegisterViewModel viewModel)
    //    {
    //        // Extraer solo los números del input
    //        string digits = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

    // if (string.IsNullOrWhiteSpace(digits)) { viewModel.NewUser.Phone = string.Empty; return; }

    // // Aplicar formato dinámico al número string formattedNumber = digits.Length switch { <= 3 =>
    // $"({digits}", <= 6 => $"({digits[..3]})-{digits[3..]}", _ =>
    // $"({digits[..3]})-{digits[3..6]}-{digits[6..]}" };

    //        // Asignar el número formateado a NewUser.Phone
    //        viewModel.NewUser.Phone = formattedNumber;
    //    }
    //}

    private void onPhoneChange(object sender, TextChangedEventArgs e)
    {
        if (sender is CustomEntryContentView entryView && entryView.BindingContext is RegisterViewModel viewModel)
        {
            viewModel.Model.Phone = FormatPhoneNumber(e.NewTextValue);

            // 🔹 Notifica a la UI que el valor cambió
            //viewModel.OnPropertyChanged(nameof(viewModel.NewUser.Phone));
        }
    }

    private string FormatPhoneNumber(string input)
    {
        string digits = new string(input.Where(char.IsDigit).ToArray());

        if (string.IsNullOrWhiteSpace(digits)) return string.Empty;

        return digits.Length switch
        {
            <= 3 => $"({digits}",
            <= 6 => $"({digits[..3]})-{digits[3..]}",
            _ => $"({digits[..3]})-{digits[3..6]}-{digits[6..]}"
        };
    }
}