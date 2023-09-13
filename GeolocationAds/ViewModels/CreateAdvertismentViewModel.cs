using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.AppTools;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class CreateAdvertismentViewModel : BaseViewModel2<Advertisement, IAdvertisementService>
    {
        private Image _image;

        public Image Image
        {
            get => _image;

            set
            {
                if (_image != value)
                {
                    _image = value;

                    OnPropertyChanged();
                }
            }
        }

        private byte[] fileBytes;

        private IAppSettingService appSettingService;

        private ObservableCollection<AppSetting> _adTypesSettings;

        public ObservableCollection<AppSetting> AdTypesSettings
        {
            get => _adTypesSettings;
            set
            {
                if (_adTypesSettings != value)
                {
                    _adTypesSettings = value;

                    OnPropertyChanged();
                }
            }
        }

        private AppSetting _selectedAdType;

        public AppSetting SelectedAdType
        {
            get => _selectedAdType;
            set
            {
                if (_selectedAdType != value)
                {
                    _selectedAdType = value;

                    //this.Model.Settings.Clear();

                    //this.Model.Settings.Add(new AdvertisementSettings()
                    //{
                    //    AdvertisementId = this.Model.ID,
                    //    SettingId = _selectedAdType.ID,
                    //    CreateDate = DateTime.Now,
                    //    CreateBy = this.LogUserPerfilTool.LogUser.ID
                    //});

                    SelectedTypeChange(_selectedAdType);

                    OnPropertyChanged();
                }
            }
        }



        public CreateAdvertismentViewModel(Advertisement advertisement, IAdvertisementService advertisementService, LogUserPerfilTool logUserPerfilTool, IAppSettingService appSettingService) : base(advertisement, advertisementService, logUserPerfilTool)
        {
            this.appSettingService = appSettingService;

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.Model.Settings = new List<AdvertisementSettings>();

            SetDefault();

            Task.Run(async () => { await this.LoadSetting(); });

            WeakReferenceMessenger.Default.Register<CleanOnSubmitMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SetDefault();
                });
            });
        }

        private void SetDefault()
        {
            this.Image = new Image();

            this.GetImageSourceFromFile();

            this.Model.UserId = this.LogUserPerfilTool.GetLogUserPropertyValue<int>("ID");
        }

        private async Task LoadSetting()
        {
            var _apiResponse = await this.appSettingService.GetAppSettingByName(SettingName.AdTypes.ToString());

            this.CollectionModel.Clear();

            if (_apiResponse.IsSuccess)
            {
                AdTypesSettings.AddRange(_apiResponse.Data);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }
        }

        [ICommand]
        private async void OnUploadCommandExecuted()
        {
            try
            {
                var fileTypes = new Dictionary<DevicePlatform, IEnumerable<string>>();

                fileTypes.Add(DevicePlatform.Android, new[] { "image/gif", "image/png", "image/jpeg" });

                var customFileTypes = new FilePickerFileType(fileTypes);

                // Pick image
                FileResult imageResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileTypes,
                    PickerTitle = "Select an image"
                });

                // Pick video
                //FileResult videoResult = await FilePicker.PickAsync(new PickOptions
                //{
                //    FileTypes = FilePickerFileType.Videos,
                //    PickerTitle = "Select a video"
                //});

                //FileResult result = imageResult ?? videoResult;

                FileResult result = imageResult;

                if (result != null)
                {
                    fileBytes = await CommonsTool.GetFileBytesAsync(result);

                    //SelectedFileLabel.Text = result.FileName;

                    //// Check if the selected file is an image or video
                    //bool isImage = result.FileName.ToLower().EndsWith(".jpg") ||
                    //               result.FileName.ToLower().EndsWith(".png") ||
                    //               result.FileName.ToLower().EndsWith(".jpeg");

                    //bool isVideo = result.FileName.ToLower().EndsWith(".mp4") ||
                    //               result.FileName.ToLower().EndsWith(".avi") ||
                    //               result.FileName.ToLower().EndsWith(".mov");

                    //if (isImage || isVideo)
                    //{
                    //    // Convert selected file to bytes
                    //    using (var stream = await result.OpenReadAsync())
                    //    {
                    //        using (MemoryStream ms = new MemoryStream())
                    //        {
                    //            await stream.CopyToAsync(ms);
                    //            fileBytes = ms.ToArray();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    // File is neither an image nor a video
                    //    await DisplayAlert("Invalid File Type", "Please select an image or video file.", "OK");
                    //}

                    var isImageSelected = result.FileName.ToLower().EndsWith(".jpg") ||
                                       result.FileName.ToLower().EndsWith(".png") ||
                                        result.FileName.ToLower().EndsWith(".gif") ||
                                       result.FileName.ToLower().EndsWith(".jpeg");

                    if (isImageSelected)
                    {
                        //Image.IsAnimationPlaying = false;

                        Image.Source = ImageSource.FromStream(() => new MemoryStream(fileBytes));

                        Image.IsAnimationPlaying = true;

                        this.Model.Content = fileBytes;

                        //if (result.FileName.ToLower().EndsWith(".gif"))
                        //{
                        //    IsAnimation = true;
                        //}
                        //Im.IsVisible = true;
                        //SelectedVideo.IsVisible = false;
                    }
                    else
                    {
                        //// Display video using WebView
                        //SelectedVideo.Source = new HtmlWebViewSource
                        //{
                        //    Html = $"<html><body><video width='100%' height='100%' controls><source src='data:video/mp4;base64,{Convert.ToBase64String(fileBytes)}' type='video/mp4' /></video></body></html>"
                        //};
                        //SelectedVideo.IsVisible = true;
                        //SelectedImage.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception

                //SelectedFileLabel.Text = "Error selecting file: " + ex.Message;

                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void GetImageSourceFromFile()
        {
            var _fileName = "mediacontent.png";

            this.Image.Source = ImageSource.FromResource($"GeolocationAds.Resources.Images.{_fileName}");

            var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(_fileName);

            this.Model.Content = _defaulMedia;
        }

        private async void SelectedTypeChange(AppSetting value)
        {
            try
            {
                if (this.Model.Settings.IsNotNullOrCountGreaterZero() && !value.IsObjectNull())
                {
                    var _toUpdateSetting = this.Model.Settings.FirstOrDefault();

                    _toUpdateSetting.UpdateDate = DateTime.Now;

                    _toUpdateSetting.UpdateBy = this.LogUserPerfilTool.LogUser.ID;

                    _toUpdateSetting.SettingId = value.ID;
                }
                else
                {
                    if (!_selectedAdType.IsObjectNull())
                    {
                        this.Model.Settings = new List<AdvertisementSettings>();

                        this.Model.Settings.Add(new AdvertisementSettings()
                        {
                            AdvertisementId = this.Model.ID,
                            SettingId = value.ID,
                            CreateDate = DateTime.Now,
                            CreateBy = this.LogUserPerfilTool.LogUser.ID
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}