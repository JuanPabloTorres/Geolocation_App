using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class ContentTypeTemplateViewModel2 : TemplateBaseViewModel2
    {
        [ObservableProperty]
        private ContentType contentType;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private ImageSource image;

        [ObservableProperty]
        private bool isAnimation;

        [ObservableProperty]
        private WebViewSource urlSource;

        private Uri _uri;

        private WebView _webView;

        public ContentVisualType ContentVisualType;

        [ObservableProperty]
        private bool canGoBack;

        [ObservableProperty]
        private bool canGoForward;

        public ContentTypeTemplateViewModel2()
        {
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, MediaSource mediaSource)
        {
            this.ContentType = contentType;

            this.MediaSource = mediaSource;

            ContentVisualType = ContentVisualType.Video;
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, ImageSource imageSource)
        {
            this.ContentType = contentType;

            this.Image = imageSource;

            ContentVisualType = ContentVisualType.Image;
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, Uri url)
        {
            this.ContentType = contentType;

            this.UrlSource = url;

            this._uri = url;

            ContentVisualType = ContentVisualType.URL;
        }

        [RelayCommand]
        public async Task ClearHistoryAndNavigate(string newUrl)
        {
            if (_webView == null || string.IsNullOrEmpty(newUrl)) return;

            // Ejecutar en el hilo principal
            await _webView.Dispatcher.DispatchAsync(async () =>
            {
                //await ClearWebViewDataAsync();
                _webView.Source = new UrlWebViewSource { Url = newUrl };
            });
        }

        //private async Task ClearWebViewDataAsync()
        //{
        //    if (_webView == null) return;

        //    await _webView.EvaluateJavaScriptAsync(@"
        //    window.localStorage.clear();
        //    window.sessionStorage.clear();
        //    history.replaceState(null, null, document.URL);
        //    window.history.pushState(null, null, document.URL);
        //    window.onpopstate = function() { history.replaceState(null, null, document.URL); };
        //");

        //    var handler = _webView.Handler?.PlatformView;
        //    if (handler == null) return;

        //    switch (DeviceInfo.Platform)
        //    {
        //        case DevicePlatform.Android:
        //            var androidWebView = handler as Android.Webkit.WebView;
        //            if (androidWebView != null)
        //            {
        //                androidWebView.ClearCache(true);
        //                androidWebView.ClearHistory();
        //            }

        //            Android.Webkit.WebStorage.Instance.DeleteAllData();
        //            Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
        //            Android.Webkit.CookieManager.Instance.Flush();
        //            break;

        //        case DevicePlatform.iOS:
        //            Foundation.NSUrlCache.SharedCache.RemoveAllCachedResponses();
        //            Foundation.NSUserDefaults.StandardUserDefaults.RemovePersistentDomain(Foundation.NSBundle.MainBundle.BundleIdentifier);
        //            Foundation.NSUserDefaults.StandardUserDefaults.Synchronize();
        //            break;
        //    }
        //}

        public async void AttachWebView(WebView webView)
        {
            _webView = webView;

            if (webView.Source is UrlWebViewSource urlSource && !string.IsNullOrEmpty(urlSource.Url))
            {
                await ClearHistoryAndNavigate(urlSource.Url);
            }
        }

        [RelayCommand]
        public async Task OpenUrl(string url)
        {
            try
            {
                await Browser.Default.OpenAsync(this._uri, BrowserLaunchMode.SystemPreferred);  // Open the URL in the system preferred browser.
            }
            catch (Exception ex)
            {
                // Handle or log exceptions that might occur (e.g., invalid URL format)
                await CommonsTool.DisplayAlert("Error", $"Could not open URL: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task Back(WebView webView)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
        }

        [RelayCommand]
        public void Forward(WebView webView)
        {
            if (webView.CanGoForward)
            {
                webView.GoForward();
            }
        }

        [RelayCommand]
        public void Reload(WebView webView)
        {
            webView?.Reload();
        }

        public void UpdateNavigationState(WebView webView)
        {
            CanGoBack = webView.CanGoBack;

            CanGoForward = webView.CanGoForward;
        }

        public async Task SetAnimation()
        {
            this.IsAnimation = false;

            await Task.Delay(10);  // Delay for 10 ms (0.01 segundos): or however long you need

            this.IsAnimation = true;      // Set to true after delay
        }

        [RelayCommand]
        public override async Task RemoveCurrentItem()
        {
            try
            {
                this.IsLoading = true;

                OnDeleteType(this);

                //EventManager2.Instance.Publish(this, CurrentPageContext);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }
    }
}