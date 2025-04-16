using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.AppTools;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.Tools;
using GeolocationAds.ViewModels;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class ContentViewTemplateViewModel : BaseTemplateViewModel
    {
        [ObservableProperty]
        private Advertisement advertisement;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private Image image;

        [ObservableProperty]
        private WebViewSource urlSource;

        private Uri _uri;

        [ObservableProperty]
        private bool isExpanded;

        public string DisplayDescription => IsExpanded ? Advertisement.Description : TruncateDescription(Advertisement.Description);

        public ContentViewTemplateViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, Advertisement advertisement, Action<ContentViewTemplateViewModel> onDelete) : base(advertisementService, geolocationAdService)
        {
            Advertisement = advertisement;

            ItemDeleted = onDelete;

            Task.Run(async () =>
            {
                await InitializeAsync();
            });
        }

        private string TruncateDescription(string description)
        {
            return description.Length > ConstantsTools.MaxLengthWithoutExpand ? description.Substring(0, ConstantsTools.MaxLengthWithoutExpand) + "..." : description;
        }

        //private string GetTruncatedDescription(string description)
        //{
        //    if (description.Length <= MaxLengthWithoutExpand)
        //        return description;

        // int endIndex = description.LastIndexOf(' ', MaxLengthWithoutExpand);

        // if (endIndex == -1 || endIndex < MaxLengthWithoutExpand / 2) { endIndex =
        // MaxLengthWithoutExpand; }

        //    return description.Substring(0, endIndex) + "...";
        //}

        //private string GetFullDescriptionWithNewLines(string description)
        //{
        //    if (description.Length <= MaxLengthWithoutExpand)
        //        return description;

        // int midIndex = description.LastIndexOf(' ', description.Length / 2);

        // if (midIndex == -1) { midIndex = description.Length / 2; }

        //    // Insertar un salto de línea en la mitad de la descripción completa
        //    return description.Insert(midIndex, "\n");
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
        public async Task OpenActionPopUp()
        {
            //try
            //{
            //    this.IsLoading = true;

            // string action = await Shell.Current.DisplayActionSheet("Actions: ", "Cancel", null,
            // "Detail", "Set Location", "Manage Location");

            // switch (action) { case "Detail":

            // await Navigate(this.Advertisement.ID);

            // break;

            // case "Set Location":

            // await SetLocationYesOrNoAlert(this.Advertisement);

            // break;

            // case "Manage Location":

            // var navigationParameter = new Dictionary<string, object> { { "ID",
            // this.Advertisement.ID } };

            // await NavigateAsync(nameof(ManageLocation), navigationParameter);

            // break;

            // default:

            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await CommonsTool.DisplayAlert("Error", ex.Message);
            //}
            //finally
            //{
            //    this.IsLoading = false;
            //}

            await RunWithLoadingIndicator(async () =>
            {
                string action = await Shell.Current.DisplayActionSheet("Actions: ", "Cancel", null, "Detail", "Set Location", "Manage Location");

                switch (action)
                {
                    case "Detail":
                        await Navigate(this.Advertisement.ID);
                        break;

                    case "Set Location":
                        await SetLocationYesOrNoAlert(this.Advertisement);
                        break;

                    case "Manage Location":
                        var navigationParameter = new Dictionary<string, object>
                        {
                            { "ID", this.Advertisement.ID }
                        };

                        await NavigateAsync(nameof(ManageLocation), navigationParameter);
                        break;

                    default:
                        break;
                }
            });
        }

        private async Task CreateAdToLocation(Advertisement ad)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var locationResponse = await GeolocationTool.GetLocation();

                if (!locationResponse.IsSuccess)
                {
                    throw new Exception(locationResponse.Message);
                }

                var newAd = GeolocationAdFactory.CreateGeolocationAd(ad, locationResponse.Data);

                var apiResponse = await geolocationAdService.Add(newAd);

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message);
                }

                Advertisement = ad;

                await CommonsTool.DisplayAlert("Notification", apiResponse.Message);
            });
        }

        private async Task RemoveContent(Advertisement ad)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var apiResponse = await advertisementService.Remove(ad.ID);

                if (!apiResponse.IsSuccess)
                {
                    await CommonsTool.DisplayAlert("Error", apiResponse.Message);
                }

                await Shell.Current.CurrentPage.ShowPopupAsync(new RemovePopUp());

                RemoveCurrentItem();

                //await CommonsTool.DisplayAlert("Notification", apiResponse.Message);
            });
        }

        private async Task SetLocationYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $"Do you want to set the current location to {selectAd.Title} ?", "Yes", "No");

                if (response)
                {
                    await this.CreateAdToLocation(selectAd);
                }
            }
        }

        private async Task Navigate(int id)
        {
            var navigationParameter = new Dictionary<string, object> { { "ID", id } };

            await Shell.Current.GoToAsync(nameof(EditAdvertisment), navigationParameter);
        }

        [RelayCommand]
        public async Task RemoveContentYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $"Are you sure you want to remove this item?", "Yes", "No");

                if (response)
                {
                    await this.RemoveContent(selectAd);
                }
            }
        }

        public override void RemoveCurrentItem()
        {
            OnDeleteItem(this);
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
        public override async Task OpenMetaDataPopUp()
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