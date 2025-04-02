using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.AppTools;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
using ToolsLibrary.Extensions;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class CaptureTemplateViewModel2 : TemplateBaseViewModel<CaptureTemplateViewModel2>
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

        public CaptureTemplateViewModel2(Capture capture, ICaptureService service, IAdvertisementService advertisementService, Action<CaptureTemplateViewModel2> onDelete) : base(advertisementService)
        {
            this.Capture = capture;

            this.Advertisement = capture.Advertisements;

            this.Service = service;

            ItemDeleted = onDelete;

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

        [RelayCommand]
        private void ToggleExpand()
        {
            IsExpanded = !IsExpanded;

            OnPropertyChanged(nameof(DisplayDescription));
        }

        public async Task InitializeAsync()
        {
            await RunWithLoadingIndicator(async () =>
            {
                var contents = Advertisement.Contents.FirstOrDefault();

                if (contents.IsObjectNull())
                {
                    contents = await AppToolCommon.GetDefaultContentType(Advertisement.CreateBy);
                }

                switch (contents.Type)
                {
                    case ContentVisualType.URL:
                        UrlSource = new Uri(contents.Url);
                        break;

                    case ContentVisualType.Image:
                        if (Image.IsObjectNull())
                        {
                            Image = new Image();
                        }

                        byte[] imageBytes = contents.Content;
                        Image.Source = AppToolCommon.LoadImageFromBytes(imageBytes);
                        break;

                    case ContentVisualType.Video:
                        var streamingResponse = await advertisementService.GetStreamingVideoUrl(contents.ID);

                        if (!streamingResponse.IsSuccess)
                        {
                            throw new Exception(streamingResponse.Message);
                        }

                        MediaSource = streamingResponse.Data;
                        break;
                }
            });
        }

       
        public override async Task RemoveCurrentItem()
        {
            await RunWithLoadingIndicator(async () =>
            {
                var confirm = await Shell.Current.DisplayAlert("Notification", "Are you sure you want to remove this item?", "Yes", "No");

                if (!confirm)
                    return;

                var apiResponse = await Service.Remove(Capture.ID);

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message);
                }

                OnDeleteType(this);
            });
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
                await Launcher.Default.TryOpenAsync(source.Url);  // Open the URL in the system preferred browser.
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
            await mediaElement.SeekTo(TimeSpan.Zero);

            mediaElement.Play();
        }

        [RelayCommand]
        public async Task GoDetail(int adId)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "ID", Advertisement.ID }
                };

                await NavigateAsync(nameof(NearByItemDetail), navigationParameter);
            });
        }

        [RelayCommand]
        public async Task OpenMetaDataPopUp()
        {
            try
            {
                var _metaDataViewModel = new MetaDataViewModel()
                {
                    CreateDate = this.Advertisement.CreateDate,
                    DataSize = this.Advertisement.Contents.FirstOrDefault().FileSize
                };

                var _metadataPopUp = new MetaDataPopUp(_metaDataViewModel);

                await Shell.Current.CurrentPage.ShowPopupAsync(_metadataPopUp);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}