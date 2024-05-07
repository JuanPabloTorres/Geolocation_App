using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.AppTools;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
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

        public delegate void MediaElementPlayingEventHandler(object sender, MediaStateChangedEventArgs e);

        public static event MediaElementPlayingEventHandler MediaElementPlaying;

        public string VideoFilePath { get; set; }

        public NearByTemplateViewModel2(ICaptureService captureService, IAdvertisementService advertisementService, Advertisement advertisement, LogUserPerfilTool logUser) : base(advertisementService)
        {
            this.LogUser = logUser;

            this.Advertisement = advertisement;

            this.service = captureService;

            Task.Run(async () =>
            {
                await InitializeAsync();
            });
        }

        //[RelayCommand]
        //private void OnMediaElementEnd(EventArgs args)
        //{
        //    // Code to execute after seek completes

        //    System.Diagnostics.Debug.WriteLine($"Completed....!");
        //}

        //[RelayCommand]
        //private void MediaStateChangedFailed(MediaFailedEventArgs args)
        //{
        //    MediaElementPlaying.Invoke(this,  null);
        //}

        //[RelayCommand]
        //private void MediaStateChanged(MediaStateChangedEventArgs args)
        //{
        //    if (
        //        args.NewState == MediaElementState.Opening

        //       )
        //    {
        //        System.Diagnostics.Debug.WriteLine($"New State (Inside If):{args.NewState}");

        //        System.Diagnostics.Debug.WriteLine($"Prevoius State (Inside If):{args.PreviousState}");

        //        //MediaElementPlaying.Invoke(this, args);
        //    }

        //    System.Diagnostics.Debug.WriteLine($"New State:{args.NewState}");

        //    System.Diagnostics.Debug.WriteLine($"Prevoius State:{args.PreviousState}");

        //    //MediaState = args.State; // Set the property to the new state
        //}

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

                            var _streamingResponse = await this.advertisementService.GetStreamingVideoUrl(content.ID);

                            if (!_streamingResponse.IsSuccess)
                            {
                                await CommonsTool.DisplayAlert("Error", _streamingResponse.Message);
                            }

                            this.MediaSource = _streamingResponse.Data;

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
            //return await CommonsTool.SaveByteArrayToTempFile2(videoData);

            VideoFilePath = await CommonsTool.SaveByteArrayToPartialFile3(videoData, string.Empty);

            //return await CommonsTool.SaveByteArrayToPartialFile3(videoData, string.Empty);

            return VideoFilePath;
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