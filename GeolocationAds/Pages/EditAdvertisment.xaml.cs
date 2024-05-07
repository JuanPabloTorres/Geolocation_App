using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditAdvertisment : ContentPage
{
    private EditAdvertismentViewModel2 viewModel;

    public EditAdvertisment(EditAdvertismentViewModel2 editAdvertismentViewModel)
    {
        InitializeComponent();

        this.viewModel = editAdvertismentViewModel;

        BindingContext = editAdvertismentViewModel;
    }
}