using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class CreateAdvertismentViewModel : BaseViewModel2<Advertisement, IAdvertisementService>
    {
        private byte[] fileBytes;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        public ObservableCollection<ContentTypeTemplateViewModel> ContentTypesTemplate { get; set; }

        private AppSetting _selectedAdType;

        public AppSetting SelectedAdType
        {
            get => _selectedAdType;
            set
            {
                if (_selectedAdType != value)
                {
                    _selectedAdType = value;

                    SelectedTypeChange(_selectedAdType);

                    OnPropertyChanged();
                }
            }
        }

        private MediaSource _mediaSource;

        public MediaSource MediaSource
        {
            get => _mediaSource;
            set
            {
                if (_mediaSource != value)
                {
                    _mediaSource = value;

                    OnPropertyChanged();
                }
            }
        }

        private IAppSettingService appSettingService;

        public ICommand UploadContentCommand { get; set; }

        public CreateAdvertismentViewModel(Advertisement advertisement, IAdvertisementService advertisementService, LogUserPerfilTool logUserPerfilTool, IAppSettingService appSettingService) : base(advertisement, advertisementService, logUserPerfilTool)
        {
            this.appSettingService = appSettingService;

            this.SelectedAdType = new AppSetting();

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.Model.Settings = new List<AdvertisementSettings>();

            this.Model.Contents = new List<ContentType>();

            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            UploadContentCommand = new Command(OnUploadCommandExecuted2);

            //this.ContentTypesTemplate.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

            //this.ContentTypesTemplate.CollectionChanged += ContentTypes_CollectionChanged;

            WeakReferenceMessenger.Default.Register<CleanOnSubmitMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.SetDefault();
                });
            });
        }

        private async void ContentTypeTemplateViewModel_ContentTypeDeleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                if (sender is ContentTypeTemplateViewModel template)
                {
                    this.ContentTypesTemplate.Remove(template);
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

        private async void ContentTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (sender is IEnumerable<ContentTypeTemplateViewModel> contents)
                {
                    if (!this.Model.Contents.IsObjectNull())
                    {
                        this.Model.Contents.Clear();

                        foreach (var item in contents)
                        {
                            this.Model.Contents.Add(item.ContentType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public async void SetDefault()
        {
            try
            {
                this.ContentTypesTemplate.Clear();

                this.ValidationResults.Clear();

                var _adSetting = new AdvertisementSettings()
                {
                    CreateDate = DateTime.Now,
                    CreateBy = this.LogUserPerfilTool.LogUser.ID,
                    SettingId = this.SelectedAdType.ID,
                };

                this.Model.UserId = this.LogUserPerfilTool.GetUserId();

                this.Model.Settings = new List<AdvertisementSettings>() { _adSetting };

                this.Model.Contents = new List<ContentType>();

                await this.GetImageSourceFromFile();
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public async Task LoadSetting()
        {
            try
            {
                this.IsLoading = true;

                this.AdTypesSettings.Clear();

                var _apiResponse = await this.appSettingService.GetAppSettingByName(SettingName.AdTypes.ToString());

                if (_apiResponse.IsSuccess)
                {
                    AdTypesSettings.AddRange(_apiResponse.Data);

                    this.SelectedAdType = AdTypesSettings.FirstOrDefault();
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
            finally
            {
                this.IsLoading = false;
            }
        }

        public async Task InitializeSettings()
        {
            await LoadSetting();
        }

        [ICommand]
        private async void OnUploadCommandExecuted2()
        {
            try
            {
                this.IsLoading = true;

                var fileTypes = new Dictionary<DevicePlatform, IEnumerable<string>>();

                fileTypes.Add(DevicePlatform.Android, new[] { "image/gif", "image/png", "image/jpeg", "video/mp4" });

                fileTypes.Add(DevicePlatform.iOS, new[] { "image/gif", "image/png", "image/jpeg", "video/mp4" });

                var customFileTypes = new FilePickerFileType(fileTypes);

                // Pick image or video
                FileResult _optionsResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileTypes,
                    PickerTitle = "Select an image or video"
                });

                FileResult result = _optionsResult;

                if (!result.IsObjectNull())
                {
                    fileBytes = await CommonsTool.GetFileBytesAsync(result);

                    var _contentType = CommonsTool.GetContentType(result.FileName);

                    if (_contentType == ContentVisualType.Image)
                    {
                        var _content = ContentTypeFactory.BuilContentType(fileBytes, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID);

                        var _template = ContentTypeTemplateFactory.BuilContentType(_content, _content.Content);

                        _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                        this.ContentTypesTemplate.Add(_template);

                        this.Model.Contents.Add(_content);
                    }

                    if (_contentType == ContentVisualType.Video)
                    {
                        var _content = ContentTypeFactory.BuilContentType(fileBytes, ContentVisualType.Video, null, this.LogUserPerfilTool.LogUser.ID);

                        var _file = CommonsTool.SaveByteArrayToTempFile(fileBytes);

                        var _template = ContentTypeTemplateFactory.BuilContentType(_content, result.FullPath);

                        _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                        this.ContentTypesTemplate.Add(_template);

                        this.Model.Contents.Add(_content);
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

        private async Task GetImageSourceFromFile()
        {
            const string FILENAME = "mediacontent.png";

            var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(FILENAME);

            if (!_defaulMedia.IsObjectNull())
            {
                var _content = ContentTypeFactory.BuilContentType(_defaulMedia, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID);

                var _template = ContentTypeTemplateFactory.BuilContentType(_content, _content.Content);

                _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                this.ContentTypesTemplate.Add(_template);
            }
        }

        private async void SelectedTypeChange(AppSetting value)
        {
            try
            {
                if (!this.Model.Settings.IsObjectNull() && !value.IsObjectNull())
                {
                    this.Model.Settings.Clear();

                    var _adSetting = new AdvertisementSettings()
                    {
                        CreateDate = DateTime.Now,
                        CreateBy = this.LogUserPerfilTool.LogUser.ID,
                        SettingId = value.ID,
                    };

                    this.Model.Settings.Add(_adSetting);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}