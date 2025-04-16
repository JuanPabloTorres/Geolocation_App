using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class MyContentPage : ContentPage
{
    private MyContentViewModel _viewModel;

    public MyContentPage(MyContentViewModel vm)
    {
        InitializeComponent();

        this._viewModel = vm;

        BindingContext = vm;

        this.paginControls.OnNextClickedAction = NextItemButton_Clicked;

        this.paginControls.OnBackClickedAction = BackItemButton_Clicked;

        //this.paginControls.IsBackButtonEnabled = false;

        this._viewModel.OnSearchExecute = OnCollectionModelChanged;
    }

    private void OnCollectionModelChanged(IEnumerable<ContentViewTemplateViewModel> sender)
    {
        if (!sender.IsObjectNull())
        {
            statusBar.IsVisible = sender.Any();
        }
    }

    private int GetSourceLastIndexCount()
    {
        return carouselView.ItemsSource.Cast<object>().ToList().Count - 1;
    }

    protected override async void OnAppearing()
    {
        if (this._viewModel.CollectionModel.Count == 0)
        {
            MyContentViewModel.PageIndex = 1;

            statusBar.IsVisible = false;

            this.paginControls.IsBackButtonEnabled = false;

            this.paginControls.IsNextButtonEnabled = true;

            this.paginControls.IsNextButtonVisible = false;

            this.paginControls.IsBackButtonVisible = false;
        }
    }

    //private async void NextItemButton_Clicked()
    //{
    //    if (_viewModel.IsLoading) return; // 🔹 Evita llamadas repetitivas si ya está cargando

    // await _viewModel.RunWithLoadingIndicator(async () => { this.paginControls.IsNextButtonEnabled
    // = false;

    // this.paginControls.IsBackButtonEnabled = false;

    // int lastIndex = GetSourceLastIndexCount();

    // // 🔹 Avanza al siguiente ítem si no está en el último if (carouselView.ItemsSource != null
    // && carouselView.Position < lastIndex) { carouselView.Position++;

    // return; }

    // // 🔹 Si se llega al final, intenta cargar más datos if (carouselView.Position == lastIndex)
    // { int oldCount = lastIndex;

    // MyContentViewModel.PageIndex++;

    // await _viewModel.InitializeAsync(MyContentViewModel.PageIndex);

    // int newCount = GetSourceLastIndexCount();

    // // 🔹 Solo avanza si hay más elementos if (newCount > oldCount) { carouselView.Position++; }
    // } });

    // this.paginControls.IsNextButtonEnabled = true;

    //    this.paginControls.IsBackButtonEnabled = true;
    //}

    private async void NextItemButton_Clicked()
    {
        await this._viewModel.RunWithLoadingIndicator(async () =>
        {
            this.paginControls.IsNextButtonEnabled = false;

            this.paginControls.IsBackButtonEnabled = false;

            if (carouselView.ItemsSource != null && carouselView.Position < GetSourceLastIndexCount())
            {
                carouselView.Position++;
            }

            if (carouselView.Position == GetSourceLastIndexCount())
            {
                var _oldCount = GetSourceLastIndexCount();

                MyContentViewModel.PageIndex++;

                await this._viewModel.InitializeAsync(MyContentViewModel.PageIndex);

                var _newCount = GetSourceLastIndexCount();

                if (_newCount > _oldCount)
                {
                    carouselView.Position++;
                }

                if (_newCount == _oldCount)
                {
                    this.paginControls.IsBackButtonEnabled = true;

                    MyContentViewModel.PageIndex--;
                }
            }

            this.paginControls.IsNextButtonEnabled = true;

            this.paginControls.IsBackButtonEnabled = true;
        });
    }

    //private async void BackItemButton_Clicked()
    //{
    //    if (_viewModel.IsLoading || carouselView.Position == 0) return; // 🔹 Evita navegación redundante

    // await _viewModel.RunWithLoadingIndicator(async () => { carouselView.Position--;

    // // 🔹 Si está en el primer elemento, desactiva el botón de retroceso
    // this.paginControls.IsBackButtonEnabled = carouselView.Position > 0;

    //        this.paginControls.IsNextButtonEnabled = true;
    //    });
    //}

    private async void BackItemButton_Clicked()
    {
        await _viewModel.RunWithLoadingIndicator(async () =>
        {
            if (carouselView.Position > 0)
            {
                carouselView.Position--;
            }

            if (carouselView.Position == 0)
            {
                this.paginControls.IsBackButtonEnabled = false;

                this.paginControls.IsNextButtonEnabled = true;
            }
            else
            {
                this.paginControls.IsBackButtonEnabled = true;

                this.paginControls.IsNextButtonEnabled = true;
            }
        });
    }
}