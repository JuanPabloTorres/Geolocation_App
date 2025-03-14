using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using System.Text.RegularExpressions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class EditAdvertisment : ContentPage
{
    private EditAdvertismentViewModel2 viewModel;

    private const int MaxRetryCount = 3;

    public EditAdvertisment(EditAdvertismentViewModel2 editAdvertismentViewModel)
    {
        InitializeComponent();

        this.viewModel = editAdvertismentViewModel;

        BindingContext = editAdvertismentViewModel;

        contentCollection.Loaded += ContentCollection_Loaded; ;

        contentCollection.Scrolled += Handle_Scrolled;
    }

    private async void Handle_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        int start = e.FirstVisibleItemIndex;

        int end = e.LastVisibleItemIndex;

        for (int i = 0; i < viewModel.ContentTypesTemplate.Count; i++)
        {
            var item = viewModel.ContentTypesTemplate[i];

            if (item.ContentVisualType == ContentVisualType.Image)
            {
                if (i >= start && i <= end)
                {
                    await item.SetAnimation();  // Call SetAnimation only for visible items
                }
            }
        }
    }

    private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (viewModel == null) return;

        viewModel.IsLoading = true; // 🔹 Activa el indicador de carga

        try
        {
            if (e.Url.StartsWith("intent://"))
            {
                var match = Regex.Match(e.Url, @"intent://(.*?)(#Intent;|$)");

                if (match.Success && !string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    string result = match.Groups[1].Value;

                    if (!result.StartsWith("http://") && !result.StartsWith("https://"))
                    {
                        result = "http://" + result; // 🔹 Asegurar que tenga un esquema válido
                    }

                    Uri uri = new Uri(result);

                    if (!await Launcher.Default.TryOpenAsync(uri))
                    {
                        await CommonsTool.DisplayAlert("Error", "Failed to open external application.");
                    }

                    e.Cancel = true; // 🔹 Evita la navegación en WebView
                    return;
                }
            }
            else if (Uri.TryCreate(e.Url, UriKind.Absolute, out Uri uri))
            {
                if (uri.Scheme == "http" || uri.Scheme == "https")
                {
                    Console.WriteLine($"Navigating to URL: {e.Url}");
                }
                else
                {
                    if (!await Launcher.Default.TryOpenAsync(uri))
                    {
                        await CommonsTool.DisplayAlert("Error", "Failed to open external application.");
                    }

                    e.Cancel = true; // 🔹 Evita la navegación en WebView
                    return;
                }
            }
            else
            {
                await CommonsTool.DisplayAlert("Error", "Invalid URL: " + e.Url);
                e.Cancel = true;
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", $"Failed to handle URL: {ex.Message}");
            e.Cancel = true;
        }
        finally
        {
            viewModel.IsLoading = false; // 🔹 Desactiva el indicador de carga al finalizar
        }
    }


    private async void ContentCollection_Loaded(object sender, EventArgs e)
    {
        var _template = (CarouselView)sender;

        foreach (var item in _template.ItemsSource.Cast<ContentTypeTemplateViewModel2>().ToList())
        {
            if (item.ContentVisualType == ContentVisualType.Image)
            {
                await item.SetAnimation();  // Call SetAnimation only for visible items
            }
        }
    }

  


}