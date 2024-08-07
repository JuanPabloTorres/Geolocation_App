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

    //private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    //{
    //    if (e.Url.StartsWith("intent://"))
    //    {
    //        try
    //        {
    //            Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");

    //            Match match = regex.Match(e.Url);

    //            string result = string.Empty;

    //            if (match.Success)
    //            {
    //                result = match.Groups[1].Value;  // Capture the URL part
    //            }

    //            if (!string.IsNullOrEmpty(result))
    //            {
    //                // Ensure the URL has a proper scheme
    //                if (!result.StartsWith("http://") && !result.StartsWith("https://"))
    //                {
    //                    result = "http://" + result;  // Default to http if no scheme is specified
    //                }

    //                Uri uri = new Uri(result);

    //                bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    //                if (!launcherOpened)
    //                    await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    //                e.Cancel = true;  // Prevent WebView from navigating
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await CommonsTool.DisplayAlert("Error", "Failed to handle URL: " + ex.Message);
    //        }
    //    }
    //    else if (Uri.TryCreate(e.Url, UriKind.Absolute, out Uri uri))
    //    {
    //        // Handle URLs that don't start with "intent://"
    //        if (uri.Scheme == "http" || uri.Scheme == "https")
    //        {
    //            // Allow WebView to navigate to standard URLs
    //            Console.WriteLine("Navigating to URL: " + e.Url);

    //            //bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);
    //            // No need to cancel navigation for http/https URLs
    //        }
    //        else
    //        {
    //            // If the URL scheme is not http/https, handle it accordingly
    //            bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    //            if (!launcherOpened)
    //                await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

    //            e.Cancel = true;  // Prevent WebView from navigating
    //        }
    //    }
    //    else
    //    {
    //        await CommonsTool.DisplayAlert("Error", "Invalid URL: " + e.Url);

    //        e.Cancel = true;  // Prevent WebView from navigating to invalid URLs
    //    }
    //}

    private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        try
        {
            if (e.Url.StartsWith("intent://"))
            {
                Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");

                Match match = regex.Match(e.Url);

                string result = string.Empty;

                if (match.Success)
                {
                    result = match.Groups[1].Value;  // Capture the URL part
                }

                if (!string.IsNullOrEmpty(result))
                {
                    // Ensure the URL has a proper scheme
                    if (!result.StartsWith("http://") && !result.StartsWith("https://"))
                    {
                        result = "http://" + result;  // Default to http if no scheme is specified
                    }

                    Uri uri = new Uri(result);

                    bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

                    if (!launcherOpened)
                        await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

                    e.Cancel = true;  // Prevent WebView from navigating
                }
            }
            else if (Uri.TryCreate(e.Url, UriKind.Absolute, out Uri uri))
            {
                // Handle URLs that don't start with "intent://"
                if (uri.Scheme == "http" || uri.Scheme == "https")
                {
                    // Allow WebView to navigate to standard URLs
                    Console.WriteLine("Navigating to URL: " + e.Url);

                    // You can add additional checks or headers if needed here
                    // e.g., e.WebRequest.Headers.Add("Custom-Header", "Value");
                }
                else
                {
                    // If the URL scheme is not http/https, handle it accordingly
                    bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

                    if (!launcherOpened)
                        await CommonsTool.DisplayAlert("Error", "Failed to open external application.");

                    e.Cancel = true;  // Prevent WebView from navigating
                }
            }
            else
            {
                await CommonsTool.DisplayAlert("Error", "Invalid URL: " + e.Url);
                e.Cancel = true;  // Prevent WebView from navigating to invalid URLs
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", "Failed to handle URL: " + ex.Message);
            e.Cancel = true;  // Prevent WebView from navigating in case of error
        }
    }


    //private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    //{
    //    if (e.Url.StartsWith("intent://"))
    //    {
    //        try
    //        {
    //            Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");

    //            Match match = regex.Match(e.Url);

    //            string result = string.Empty;

    //            if (match.Success)
    //            {
    //                result = match.Groups[1].Value;  // Capture the URL part

    //                //Console.WriteLine("Extracted URL: " + result);
    //            }

    //            if (!string.IsNullOrEmpty(result))
    //            {
    //                // Ensure the URL has a proper scheme
    //                if (!result.StartsWith("http://") && !result.StartsWith("https://"))
    //                {
    //                    result = "http://" + result;  // Default to http if no scheme is specified
    //                }

    //                Uri uri = new Uri(result);

    //                bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    //                if (launcherOpened)
    //                {
    //                    //Console.WriteLine("External application launched successfully.");
    //                }
    //                else
    //                {
    //                    //Console.WriteLine("Failed to open external application.");
    //                }

    //                e.Cancel = true;  // Prevent WebView from navigating
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Console.WriteLine("Failed to handle URL: " + ex.Message);

    //            //await Shell.Current.DisplayAlert("Error", ex.Message, "OK");

    //            await CommonsTool.DisplayAlert("Error", ex.Message);
    //        }
    //    }
    //    else if (Uri.TryCreate(e.Url, UriKind.Absolute, out Uri uri))
    //    {
    //        // Handle URLs that don't start with "intent://"
    //        if (uri.Scheme == "http" || uri.Scheme == "https")
    //        {
    //            // Allow WebView to navigate to standard URLs
    //            Console.WriteLine("Navigating to URL: " + e.Url);

    //        }
    //        else
    //        {
    //            // If the URL scheme is not http/https, handle it accordingly
    //            bool launcherOpened = await Launcher.Default.TryOpenAsync(uri);

    //            if (launcherOpened)
    //            {
    //                Console.WriteLine("External application launched successfully.");
    //            }
    //            else
    //            {
    //                Console.WriteLine("Failed to open external application.");

    //            }

    //            e.Cancel = true;  // Prevent WebView from navigating
    //        }

    //        //if (e.Url.StartsWith("intent://"))
    //        //{
    //        //    try
    //        //    {
    //        //        Regex regex = new Regex(@"intent://(.*?)(;|$)");

    //        //        Match match = regex.Match(e.Url);

    //        //        string result = string.Empty;

    //        //        if (match.Success)
    //        //        {
    //        //            result = match.Groups[1].Value;  // Capture the URL part

    //        //            Console.WriteLine(result);
    //        //        }

    //        //        if (result != string.Empty)
    //        //        {
    //        //            //var uri = new Uri(result);

    //        //            bool launcherOpened = await Launcher.Default.TryOpenAsync(result);

    //        //            if (launcherOpened)
    //        //            {
    //        //                // Do something fun
    //        //            }

    //        //            //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);  // Open the URL in the system preferred browser.

    //        //            e.Cancel = true;  // Prevent WebView from navigating
    //        //        }

    //        //        //var uri = new Uri(result);

    //        //        //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);  // Open the URL in the system preferred browser.

    //        //        //e.Cancel = true;  // Prevent WebView from navigating
    //        //    }
    //        //    catch (Exception ex)
    //        //    {
    //        //        Console.WriteLine("Failed to open URL: " + ex.Message);
    //        //    }
    //        //}
    //    }
    //}

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

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        //var webView = sender as WebView;

        //if (e.Result == WebNavigationResult.Failure)
        //{
        //    await HandleNavigationFailureAsync(e.Url, webView);
        //}
    }

    private async Task HandleNavigationFailureAsync(string url, WebView webView)
    {
        int retryCount = 0;

        bool success = false;

        while (retryCount < MaxRetryCount && !success)
        {
            try
            {
                Console.WriteLine($"Retrying to load URL: {url} (Attempt {retryCount + 1}/{MaxRetryCount})");

                webView.Source = url;

                success = true;
            }
            catch
            {
                retryCount++;
                await Task.Delay(1000); // Optional: Add a delay before retrying
            }
        }

        if (!success)
        {
            Console.WriteLine($"Failed to load URL after {MaxRetryCount} attempts: {url}");

            await DisplayAlert("Error", $"Failed to load URL: {url}", "OK");
        }
    }
}