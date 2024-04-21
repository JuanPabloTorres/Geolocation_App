using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.AppTools;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class NearByTemplateViewModel2 : BaseTemplateViewModel
    {
        [ObservableProperty]
        private Advertisement advertisement;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private Image image;

        protected readonly LogUserPerfilTool LogUser;

        protected readonly ICaptureService service;

        public NearByTemplateViewModel2(ICaptureService captureService, Advertisement advertisement, LogUserPerfilTool logUser)
        {
            this.LogUser = logUser;

            this.Advertisement = advertisement;

            this.service = captureService;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var content = this.Advertisement.Contents.FirstOrDefault();

                if (content != null)
                {
                    switch (content.Type)
                    {
                        case ContentVisualType.Image:

                            this.Image = new Image();

                            this.Image.Source = await LoadImageAsync(content.Content);

                            break;

                        case ContentVisualType.Video:

                            var file = await SaveToTempAsync(content.Content);

                            this.MediaSource = file;

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private async Task<ImageSource> LoadImageAsync(byte[] imageData)
        {
            return await Task.Run(() => AppToolCommon.LoadImageFromBytes(imageData));
        }

        private async Task<MediaSource> SaveToTempAsync(byte[] videoData)
        {
            return await CommonsTool.SaveByteArrayToTempFile2(videoData);
        }

        public async Task FillTemplate()
        {
            try
            {
                if (!this.Advertisement.Contents.IsObjectNull())
                {
                    if (this.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Image)
                    {
                        if (this.Image.IsObjectNull())
                        {
                            this.Image = new Image();
                        }

                        var _imageBytes = this.Advertisement.Contents.FirstOrDefault().Content;

                        Image.Source = AppToolCommon.LoadImageFromBytes(_imageBytes);
                    }

                    if (this.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Video)
                    {
                        var file = await CommonsTool.SaveByteArrayToTempFile2(this.Advertisement.Contents.FirstOrDefault().Content);

                        this.MediaSource = file;
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        [RelayCommand]
        public async Task CaptureItem(Advertisement advertisement)
        {
            try
            {
                var _capture = new Capture()
                {
                    AdvertisementId = this.Advertisement.ID,
                    UserId = this.LogUser.GetUserId(),
                    CreateDate = DateTime.Now,
                    CreateBy = this.LogUser.GetUserId(),
                };

                var _apiResponse = await this.service.Add(_capture);

                if (_apiResponse.IsSuccess)
                {
                    await CommonsTool.DisplayAlert("Notification", "Capture completed successfully.");
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}