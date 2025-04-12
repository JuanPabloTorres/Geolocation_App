
using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using System.Text.RegularExpressions;
using ToolsLibrary.Tools;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    private CreateAdvertismentViewModel2 viewModel;

    private string _lastNavigatedUrl;

    public CreateAdvertisment(CreateAdvertismentViewModel2 createGeolocationViewModel)
    {
        InitializeComponent();

        viewModel = createGeolocationViewModel;


        BindingContext = createGeolocationViewModel;
    }

    protected override async void OnAppearing()
    {
        if (!viewModel.AdTypesSettings.Any())
        {
            await viewModel.InitializeAsync();
        }

        await this.viewModel.SetDefault();
    }



    private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        try
        {
            if (e.Url.StartsWith("intent://"))
            {
                Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");

                Match match = regex.Match(e.Url);

                string result = match.Success ? match.Groups[1].Value : string.Empty;

                if (!string.IsNullOrEmpty(result))
                {
                    if (!result.StartsWith("http://") && !result.StartsWith("https://"))
                    {
                        result = "http://" + result;
                    }

                    bool launcherOpened = await Launcher.Default.TryOpenAsync(new Uri(result));

                    if (!launcherOpened)
                        await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

                    e.Cancel = true;
                }
            }
            else if (Uri.TryCreate(e.Url, UriKind.Absolute, out Uri uri))
            {
                if (uri.Scheme == "http" || uri.Scheme == "https")
                {
                    Console.WriteLine("Navigating to URL: " + e.Url);
                }
                else
                {
                    bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

                    if (!launcherOpened)
                        await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

                    e.Cancel = true;
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
            await CommonsTool.DisplayAlert("Error", "Failed to handle URL: " + ex.Message);
            e.Cancel = true;
        }
    }

    private void OnWebViewLoaded(object sender, EventArgs e)
    {
        if (sender is WebView webView && webView.BindingContext is ContentTypeTemplateViewModel2 viewModel)
        {
            viewModel.AttachWebView(webView);
        }
    }

    //private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    //{
    //    try
    //    {
    //        if (sender is WebView webView)
    //        {
    //            // ?? Verifica si la URL es diferente antes de limpiar el caché
    //            if (webView.Source is UrlWebViewSource currentSource && currentSource.Url != e.Url)
    //            {
    //                ClearWebViewCacheAndHistory(webView);
    //            }
    //        }

    // if (e.Url.StartsWith("intent://")) { Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");
    // Match match = regex.Match(e.Url);

    // string result = match.Success ? match.Groups[1].Value : string.Empty;

    // if (!string.IsNullOrEmpty(result)) { if (!result.StartsWith("http://") &&
    // !result.StartsWith("https://")) { result = "http://" + result; }

    // bool launcherOpened = await Launcher.Default.TryOpenAsync(new Uri(result));

    // if (!launcherOpened) await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    // e.Cancel = true; } } else if (Uri.TryCreate(e.Url, UriKind.Absolute, out Uri uri)) { if
    // (uri.Scheme == "http" || uri.Scheme == "https") { Console.WriteLine("Navigating to URL: " +
    // e.Url); } else { bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    // if (!launcherOpened) await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    //                e.Cancel = true;
    //            }
    //        }
    //        else
    //        {
    //            await CommonsTool.DisplayAlert("Error", "Invalid URL: " + e.Url);
    //            e.Cancel = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", "Failed to handle URL: " + ex.Message);
    //        e.Cancel = true;
    //    }
    //}

    //private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    //{
    //    try
    //    {
    //        if (sender is WebView webView)
    //        {
    //            // ?? Limpia el caché del WebView antes de navegar
    //            ClearWebViewCache(webView);
    //        }

    // if (e.Url.StartsWith("intent://")) { Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");
    // Match match = regex.Match(e.Url);

    // string result = string.Empty;

    // if (match.Success) { result = match.Groups[1].Value; }

    // if (!string.IsNullOrEmpty(result)) { if (!result.StartsWith("http://") &&
    // !result.StartsWith("https://")) { result = "http://" + result; }

    // Uri uri = new Uri(result); bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    // if (!launcherOpened) await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    // e.Cancel = true; } } else if (Uri.TryCreate(e.Url, UriKind.Absolute, out Uri uri)) { if
    // (uri.Scheme == "http" || uri.Scheme == "https") { Console.WriteLine("Navigating to URL: " +
    // e.Url); } else { bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    // if (!launcherOpened) await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    //                e.Cancel = true;
    //            }
    //        }
    //        else
    //        {
    //            await CommonsTool.DisplayAlert("Error", "Invalid URL: " + e.Url);
    //            e.Cancel = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", "Failed to handle URL: " + ex.Message);
    //        e.Cancel = true;
    //    }
    //}

    //private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Url.StartsWith("intent://"))
    //        {
    //            Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");
    //            Match match = regex.Match(e.Url);

    // string result = string.Empty;

    // if (match.Success) { result = match.Groups[1].Value; // Capture the URL part }

    // if (!string.IsNullOrEmpty(result)) { // Ensure the URL has a proper scheme if
    // (!result.StartsWith("http://") && !result.StartsWith("https://")) { result = "http://" +
    // result; // Default to http if no scheme is specified }

    // Uri uri = new Uri(result);

    // bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    // if (!launcherOpened) await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    // e.Cancel = true; // Prevent WebView from navigating } } else if (Uri.TryCreate(e.Url,
    // UriKind.Absolute, out Uri uri)) { // Handle URLs that don't start with "intent://" if
    // (uri.Scheme == "http" || uri.Scheme == "https") { // Allow WebView to navigate to standard
    // URLs Console.WriteLine("Navigating to URL: " + e.Url);

    // // You can add additional checks or headers if needed here // e.g.,
    // e.WebRequest.Headers.Add("Custom-Header", "Value"); } else { // If the URL scheme is not
    // http/https, handle it accordingly bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    // if (!launcherOpened) await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    //                e.Cancel = true;  // Prevent WebView from navigating
    //            }
    //        }
    //        else
    //        {
    //            await CommonsTool.DisplayAlert("Error", "Invalid URL: " + e.Url);
    //            e.Cancel = true;  // Prevent WebView from navigating to invalid URLs
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", "Failed to handle URL: " + ex.Message);
    //        e.Cancel = true;  // Prevent WebView from navigating in case of error
    //    }
    //}

    //private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    //{
    //    if (e.Url.StartsWith("intent://"))
    //    {
    //        try
    //        {
    //            Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");

    // Match match = regex.Match(e.Url);

    // string result = string.Empty;

    // if (match.Success) { result = match.Groups[1].Value; // Capture the URL part

    // Console.WriteLine("Extracted URL: " + result); }

    // if (!string.IsNullOrEmpty(result)) { // Ensure the URL has a proper scheme if
    // (!result.StartsWith("http://") && !result.StartsWith("https://")) { result = "http://" +
    // result; // Default to http if no scheme is specified }

    // Uri uri = new Uri(result);

    // bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    // if (launcherOpened) { Console.WriteLine("External application launched successfully."); }
    // else { Console.WriteLine("Failed to open external application."); }

    // e.Cancel = true; // Prevent WebView from navigating } } catch (Exception ex) {
    // Console.WriteLine("Failed to handle URL: " + ex.Message); } }

    // //if (e.Url.StartsWith("intent://")) //{ // try // { // Regex regex = new Regex(@"intent://(.*?)(;|$)");

    // // Match match = regex.Match(e.Url);

    // // string result = string.Empty;

    // // if (match.Success) // { // result = match.Groups[1].Value; // Capture the URL part

    // // Console.WriteLine(result); // }

    // // if (result != string.Empty) // { // //var uri = new Uri(result);

    // // bool launcherOpened = await Launcher.Default.TryOpenAsync(result);

    // // if (launcherOpened) // { // // Do something fun // }

    // // //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred); // Open the URL
    // in the system preferred browser.

    // // e.Cancel = true; // Prevent WebView from navigating // }

    // // //var uri = new Uri(result);

    // // //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred); // Open the URL
    // in the system preferred browser.

    //    //        //e.Cancel = true;  // Prevent WebView from navigating
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        Console.WriteLine("Failed to open URL: " + ex.Message);
    //    //    }
    //    //}
    //}
}