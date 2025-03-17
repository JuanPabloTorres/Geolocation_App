using GeolocationAds.ViewModels;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class MyContentPage : ContentPage
{
    private MyContentViewModel2 _viewModel;

    public MyContentPage(MyContentViewModel2 vm)
    {
        InitializeComponent();

        this._viewModel = vm;

        BindingContext = vm;

        this.paginControls.NextClicked += NextItemButton_Clicked;

        this.paginControls.BackClicked += BackItemButton_Clicked;

        this.paginControls.IsBackButtonEnabled = false;

        this.paginControls.IsNextButtonEnabled = true;
    }

    private int GetSourceLastIndexCount()
    {
        return carouselView.ItemsSource.Cast<object>().ToList().Count - 1;
    }

    private async void NextItemButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.IsLoading) return; // 🔹 Evita llamadas repetitivas si ya está cargando

        await _viewModel.RunWithLoadingIndicator(async () =>
        {
            this.paginControls.IsNextButtonEnabled = false;

            this.paginControls.IsBackButtonEnabled = false;

            int lastIndex = GetSourceLastIndexCount();

            // 🔹 Avanza al siguiente ítem si no está en el último
            if (carouselView.ItemsSource != null && carouselView.Position < lastIndex)
            {
                carouselView.Position++;

                return;
            }

            // 🔹 Si se llega al final, intenta cargar más datos
            if (carouselView.Position == lastIndex)
            {
                int oldCount = lastIndex;

                MyContentViewModel2.PageIndex++;

                await _viewModel.InitializeAsync(MyContentViewModel2.PageIndex);

                int newCount = GetSourceLastIndexCount();

                // 🔹 Solo avanza si hay más elementos
                if (newCount > oldCount)
                {
                    carouselView.Position++;
                }
            }
        });

        this.paginControls.IsNextButtonEnabled = true;

        this.paginControls.IsBackButtonEnabled = true;
    }

    private async void BackItemButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.IsLoading || carouselView.Position == 0) return; // 🔹 Evita navegación redundante

        await _viewModel.RunWithLoadingIndicator(async () =>
        {
            carouselView.Position--;

            // 🔹 Si está en el primer elemento, desactiva el botón de retroceso
            this.paginControls.IsBackButtonEnabled = carouselView.Position > 0;

            this.paginControls.IsNextButtonEnabled = true;
        });
    }

    private void OnPageLoaded(object sender, EventArgs e)
    {

    }
}