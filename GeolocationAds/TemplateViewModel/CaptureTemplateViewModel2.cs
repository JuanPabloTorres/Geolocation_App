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
    }
}