using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class EditAdvertisment : ContentPage
{
    private EditAdvertismentViewModel viewModel;

    public EditAdvertisment(EditAdvertismentViewModel editAdvertismentViewModel)
    {
        InitializeComponent();

        this.viewModel = editAdvertismentViewModel;

        BindingContext = editAdvertismentViewModel;
    }


}