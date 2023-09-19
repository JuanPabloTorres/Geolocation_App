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

        private ObservableCollection<ContentTypeTemplateViewModel> _contentTypestemplate;

        public ObservableCollection<ContentTypeTemplateViewModel> ContentTypesTemplate
        {
            get => _contentTypestemplate;
            set
            {
                if (_contentTypestemplate != value)
                {
                    _contentTypestemplate = value;

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

                    SelectedTypeChange(_selectedAdType);

                    OnPropertyChanged();
                }
            }
        }

        private IAppSettingService appSettingService;

        public ICommand UploadContentCommand { get; set; }

        public CreateAdvertismentViewModel(Advertisement advertisement, IAdvertisementService advertisementService, LogUserPerfilTool logUserPerfilTool, IAppSettingService appSettingService) : base(advertisement, advertisementService, logUserPerfilTool)
        {
            this.appSettingService = appSettingService;

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.Model.Settings = new List<AdvertisementSettings>();

            this.Model.Contents = new List<ContentType>();

            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            UploadContentCommand = new Command(OnUploadCommandExecuted2);

            //Task.Run(async () => { await this.LoadSetting(); });

            ContentTypeTemplateViewModel.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

            this.ContentTypesTemplate.CollectionChanged += ContentTypes_CollectionChanged;

            SetDefault();

            WeakReferenceMessenger.Default.Register<CleanOnSubmitMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.ContentTypesTemplate.Clear();

                    SetDefault();
                });
            });
        }

        private void ContentTypeTemplateViewModel_ContentTypeDeleted(object sender, EventArgs e)
        {
            this.IsLoading = true;

            if (sender is ContentTypeTemplateViewModel template)
            {
                if (!template.IsObjectNull())
                {
                    this.ContentTypesTemplate.Remove(template);
                }
            }

            this.IsLoading = false;
        }

        private void ContentTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sender is IList<ContentTypeTemplateViewModel> contents)
            {
                if (!this.Model.Contents.IsObjectNull())
                {
                    if (this.Model.Contents.Count > 0)
                    {
                        this.Model.Contents.Clear();
                    }

                    foreach (var item in contents)
                    {
                        //var _toModelContent = new ContentType()
                        //{
                        //    Content = CommonsTool.Compress(item.ContentType.Content),
                        //    AdvertisingId = item.ContentType.AdvertisingId,
                        //    CreateDate = item.ContentType.CreateDate,
                        //    Type = item.ContentType.Type,
                        //    CreateBy = this.LogUserPerfilTool.LogUser.ID
                        //};

                        this.Model.Contents.Add(item.ContentType);
                    }
                }
            }
        }

        private void SetDefault()
        {
            if (this.Model.Contents.IsObjectNull())
            {
                this.Model.Contents = new List<ContentType>();
            }

            this.GetImageSourceFromFile();

            this.Model.UserId = this.LogUserPerfilTool.GetLogUserPropertyValue<int>("ID");
        }

        public async Task LoadSetting()
        {
            var _apiResponse = await this.appSettingService.GetAppSettingByName(SettingName.AdTypes.ToString());

            this.AdTypesSettings.Clear();

            if (_apiResponse.IsSuccess)
            {
                AdTypesSettings.AddRange(_apiResponse.Data);

                this.SelectedAdType = AdTypesSettings.FirstOrDefault();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }
        }

        [ICommand]
        private async void OnUploadCommandExecuted2()
        {
            try
            {
                var fileTypes = new Dictionary<DevicePlatform, IEnumerable<string>>();

                fileTypes.Add(DevicePlatform.Android, new[] { "image/gif", "image/png", "image/jpeg", "video/mp4" });

                fileTypes.Add(DevicePlatform.iOS, new[] { "image/gif", "image/png", "image/jpeg", "video/mp4" });

                var customFileTypes = new FilePickerFileType(fileTypes);

                // Pick image or video
                FileResult imageResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileTypes,
                    PickerTitle = "Select an image or video"
                });

                FileResult result = imageResult;

                if (result != null)
                {
                    fileBytes = await CommonsTool.GetFileBytesAsync(result);

                    var isImageSelected = result.FileName.ToLower().EndsWith(".jpg") ||
                        result.FileName.ToLower().EndsWith(".png") ||
                        result.FileName.ToLower().EndsWith(".gif") ||
                        result.FileName.ToLower().EndsWith(".jpeg");

                    if (isImageSelected)
                    {
                        var _content = ContentTypeFactory.BuilContentType(fileBytes, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID);

                        var _template = ContentTypeTemplateFactory.BuilContentType(_content);

                        this.ContentTypesTemplate.Add(_template);
                    }
                    else
                    {
                        var _content = ContentTypeFactory.BuilContentType(fileBytes, ContentVisualType.Video, null, this.LogUserPerfilTool.LogUser.ID);

                        var _template = ContentTypeTemplateFactory.BuilContentType(_content);

                        this.ContentTypesTemplate.Add(_template);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void GetImageSourceFromFile()
        {
            const string FILENAME = "mediacontent.png";

            var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(FILENAME);

            var _content = ContentTypeFactory.BuilContentType(_defaulMedia, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID);

            var _template = ContentTypeTemplateFactory.BuilContentType(_content);

            this.ContentTypesTemplate.Add(_template);
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}