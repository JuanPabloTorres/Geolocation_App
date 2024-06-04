using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using System.Text.RegularExpressions;
using ToolsLibrary.Models;

namespace GeolocationAds.Pages;

public partial class NearByItemDetail : ContentPage
{

    NearByItemDetailViewModel viewModel;

    public NearByItemDetail(NearByItemDetailViewModel nearByItemDetailViewModel)
    {
        InitializeComponent();

        this.viewModel = nearByItemDetailViewModel;

        this.BindingContext = viewModel;

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
        if (e.Url.StartsWith("intent://"))
        {
            try
            {
                Regex regex = new Regex(@"intent://(.*?)(#Intent;|$)");

                Match match = regex.Match(e.Url);

                string result = string.Empty;

                if (match.Success)
                {
                    result = match.Groups[1].Value;  // Capture the URL part

                    Console.WriteLine("Extracted URL: " + result);
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

                    if (launcherOpened)
                    {
                        Console.WriteLine("External application launched successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to open external application.");
                    }

                    e.Cancel = true;  // Prevent WebView from navigating
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to handle URL: " + ex.Message);
            }
        }

        //if (e.Url.StartsWith("intent://"))
        //{
        //    try
        //    {
        //        Regex regex = new Regex(@"intent://(.*?)(;|$)");

        //        Match match = regex.Match(e.Url);

        //        string result = string.Empty;

        //        if (match.Success)
        //        {
        //            result = match.Groups[1].Value;  // Capture the URL part

        //            Console.WriteLine(result);
        //        }

        //        if (result != string.Empty)
        //        {
        //            //var uri = new Uri(result);

        //            bool launcherOpened = await Launcher.Default.TryOpenAsync(result);

        //            if (launcherOpened)
        //            {
        //                // Do something fun
        //            }

        //            //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);  // Open the URL in the system preferred browser.

        //            e.Cancel = true;  // Prevent WebView from navigating
        //        }

        //        //var uri = new Uri(result);

        //        //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);  // Open the URL in the system preferred browser.

        //        //e.Cancel = true;  // Prevent WebView from navigating
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Failed to open URL: " + ex.Message);
        //    }
        //}
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