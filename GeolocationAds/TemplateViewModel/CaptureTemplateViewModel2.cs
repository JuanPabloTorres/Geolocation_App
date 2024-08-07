using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.AppTools;
using GeolocationAds.Services;
using ToolsLibrary.Extensions;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class CaptureTemplateViewModel2 : TemplateBaseViewModel2
    {
        public ICaptureService Service { get; set; }

        [ObservableProperty]
        private Advertisement advertisement;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private Image image;

        [ObservableProperty]
        private WebViewSource urlSource;

        [ObservableProperty]
        private Capture capture;

        [ObservableProperty]
        private bool isExpanded;

        private const int MaxLengthWithoutExpand = 100;

        public string DisplayDescription => IsExpanded ? Advertisement.Description : TruncateDescription(Advertisement.Description);

        public CaptureTemplateViewModel2(Capture capture, ICaptureService service, IAdvertisementService advertisementService) : base(advertisementService)
        {
            this.Capture = capture;

            this.Advertisement = capture.Advertisements;

            this.Service = service;

            Task.Run(async () =>
            {
                await InitializeAsync();
            });
        }

        private string TruncateDescription(string description)
        {
            const int maxLength = 100; // Adjust the length as needed

            return description.Length > maxLength ? description.Substring(0, maxLength) + "..." : description;
        }

        private string GetTruncatedDescription(string description)
        {
            if (description.Length <= MaxLengthWithoutExpand)
                return description;

            int endIndex = description.LastIndexOf(' ', MaxLengthWithoutExpand);

            if (endIndex == -1 || endIndex < MaxLengthWithoutExpand / 2)
            {
                endIndex = MaxLengthWithoutExpand;
            }

            return description.Substring(0, endIndex) + "...";
        }

        private string GetFullDescriptionWithNewLines(string description)
        {
            if (description.Length <= MaxLengthWithoutExpand)
                return description;

            int midIndex = description.LastIndexOf(' ', description.Length / 2);

            if (midIndex == -1)
            {
                midIndex = description.Length / 2;
            }

            // Insertar un salto de línea en la mitad de la descripción completa
            return description.Insert(midIndex, "\n");
        }

        //private string GetTruncatedDescription(string description)
        //{
        //    if (description.Length <= MaxLengthWithoutExpand)
        //        return description;

        //    int midIndex = description.Length / 2;

        //    string truncated = description.Substring(0, midIndex) + "\n" + description.Substring(midIndex);

        //    return truncated.Length > MaxLengthWithoutExpand ? truncated.Substring(0, MaxLengthWithoutExpand) + "..." : truncated;
        //}

        [RelayCommand]
        private void ToggleExpand()
        {
            IsExpanded = !IsExpanded;

            OnPropertyChanged(nameof(DisplayDescription));
        }

        public async Task InitializeAsync()
        {
            try
            {
                var contents = this.Advertisement.Contents.FirstOrDefault();

                if (contents == null)
                {
                    contents = await AppToolCommon.GetDefaultContentType(this.Advertisement.CreateBy);
                }

                switch (contents.Type)
                {
                    case ContentVisualType.URL:

                        this.UrlSource = new Uri(contents.Url);

                        break;

                    case ContentVisualType.Image:

                        if (this.Image.IsObjectNull())
                        {
                            this.Image = new Image();
                        }

                        byte[] imageBytes = contents.Content;

                        this.Image.Source = AppToolCommon.LoadImageFromBytes(imageBytes);

                        break;

                    case ContentVisualType.Video:

                        var streamingResponse = await this.advertisementService.GetStreamingVideoUrl(contents.ID);

                        if (!streamingResponse.IsSuccess)
                        {
                            await CommonsTool.DisplayAlert("Error", streamingResponse.Message);

                            return;
                        }

                        this.MediaSource = streamingResponse.Data;

                        break;
                }
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

        [RelayCommand]
        public override async Task RemoveCurrentItem()
        {
            try
            {
                this.IsLoading = true;

                var _removeResponse = await Shell.Current.DisplayAlert("Notification", $"Are you sure you want to remove this item?", "Yes", "No");

                if (_removeResponse)
                {
                    var _apiResponse = await this.Service.Remove(this.Capture.ID);

                    if (_apiResponse.IsSuccess)
                    {
                        //OnDeleteType(EventArgs.Empty);

                        EventManager2.Instance.Publish(this, CurrentPageContext);
                    }
                    else
                    {
                        await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                    }
                }
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

        [RelayCommand]
        public async Task OpenUrl(UrlWebViewSource source)
        {
            try
            {
                await Browser.Default.OpenAsync(source.Url, BrowserLaunchMode.SystemPreferred);  // Open the URL in the system preferred browser.
            }
            catch (Exception ex)
            {
                // Handle or log exceptions that might occur (e.g., invalid URL format)
                await CommonsTool.DisplayAlert("Error", $"Could not open URL: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task BackMedia(MediaElement mediaElement)
        {
            var newPosition = mediaElement.Position - TimeSpan.FromSeconds(10);

            await mediaElement.SeekTo(newPosition < TimeSpan.Zero ? TimeSpan.Zero : newPosition);
        }

        [RelayCommand]
        public async Task ForwardMedia(MediaElement mediaElement)
        {
            var newPosition = mediaElement.Position + TimeSpan.FromSeconds(10);

            await mediaElement.SeekTo(newPosition > mediaElement.Duration ? mediaElement.Duration : newPosition);
        }

        [RelayCommand]
        public void PlayPause(MediaElement mediaElement)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                mediaElement.Pause();
            }
            else
            {
                mediaElement.Play();
            }
        }

        [RelayCommand]
        public async Task ReloadMedia(MediaElement mediaElement)
        {
            //mediaElement.Stop();

            //mediaElement.Play();

            await mediaElement.SeekTo(TimeSpan.Zero);

            mediaElement.Play();
        }
    }
}