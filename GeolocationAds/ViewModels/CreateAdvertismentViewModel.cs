using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using Xabe.FFmpeg;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

using Xabe.FFmpeg;

namespace GeolocationAds.ViewModels
{
    public partial class CreateAdvertismentViewModel : BaseViewModel2<Advertisement, IAdvertisementService>
    {
        private const string FILENAME = "mediacontent.png";

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

        private readonly IAppSettingService appSettingService;

        public ICommand UploadContentCommand { get; set; }

        public CreateAdvertismentViewModel(Advertisement advertisement, IAdvertisementService advertisementService, LogUserPerfilTool logUserPerfilTool, IAppSettingService appSettingService) : base(advertisement, advertisementService, logUserPerfilTool)
        {
            this.appSettingService = appSettingService;

            this.SelectedAdType = new AppSetting();

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            UploadContentCommand = new Command(OnUploadCommandExecuted2);

            WeakReferenceMessenger.Default.Register<CleanOnSubmitMessage<Advertisement>>(this, async (r, m) =>
            {
                this.SetDefault();
            });
        }

        private async void ContentTypeTemplateViewModel_ContentTypeDeleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                if (sender is ContentTypeTemplateViewModel template)
                {
                    if (this.ContentTypesTemplate.Count() == 1)
                    {
                        await CommonsTool.DisplayAlert("Error", "At least one item is required.You may remove any excess items.");
                    }
                    else
                    {
                        this.ContentTypesTemplate.Remove(template);
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

        public async void SetDefault()
        {
            try
            {
                this.IsLoading = true;

                var _adSetting = new AdvertisementSettings()
                {
                    CreateDate = DateTime.Now,
                    CreateBy = this.LogUserPerfilTool.LogUser.ID,
                    SettingId = this.SelectedAdType.ID,
                };

                this.Model = new Advertisement()
                {
                    UserId = this.LogUserPerfilTool.GetUserId(),
                    CreateDate = DateTime.Now,
                    Settings = new List<AdvertisementSettings>() { _adSetting },
                    Contents = new List<ContentType>()
                };

                await this.GetImageSourceFromFile();
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

        private async void OnUploadCommandExecuted2()
        {
            try
            {
                this.IsLoading = true;

                var customFileTypes = GetCommonFileTypes();

                FileResult result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileTypes,
                });

                if (!result.IsObjectNull())
                {
                    await ProcessSelectedFile(result);
                }
            }
            catch (Exception ex) // Consider catching more specific exceptions
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private FilePickerFileType GetCommonFileTypes()
        {
            var fileTypes = new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        { DevicePlatform.Android, new[] { "image/gif", "image/png", "image/jpeg", "video/mp4" } },
        { DevicePlatform.iOS, new[] { "image/gif", "image/png", "image/jpeg", "video/mp4" } }
    };

            return new FilePickerFileType(fileTypes);
        }

        private async Task ProcessSelectedFile(FileResult result)
        {
            var fileBytes = await CommonsTool.GetFileBytesAsync(result);

            var contentType = CommonsTool.GetContentType(result.FileName);

            switch (contentType)
            {
                case ContentVisualType.Image:
                    await ProcessImageContent(fileBytes, result);
                    break;

                case ContentVisualType.Video:

                    await ProcessVideoContent(fileBytes, result);

                    break;
            }

            RemoveDefaultImageIfPresent();
        }

        private async Task ProcessImageContent(byte[] fileBytes, FileResult result)
        {
            var _fileBytes = await CommonsTool.GetFileBytesAsync(result);

            var _contentType = CommonsTool.GetContentType(result.FileName);

            if (_contentType == ContentVisualType.Image)
            {
                var _content = ContentTypeFactory.BuilContentType(_fileBytes, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID, result.FileName);

                var _template = ContentTypeTemplateFactory.BuilContentType(_content, _content.Content);

                _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                this.ContentTypesTemplate.Add(_template);

                this.Model.Contents.Add(_content);
            }
        }

        private async Task ProcessVideoContent(byte[] fileBytes, FileResult result)
        {
            var _fileBytes = await CommonsTool.GetFileBytesAsync(result);

            var _contentType = CommonsTool.GetContentType(result.FileName);

            var _content = ContentTypeFactory.BuilContentType(_fileBytes, ContentVisualType.Video, null, this.LogUserPerfilTool.LogUser.ID, result.FileName);

            var _file = CommonsTool.SaveByteArrayToTempFile(_fileBytes);

            var _template = ContentTypeTemplateFactory.BuilContentType(_content, result.FullPath);

            _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

            this.ContentTypesTemplate.Add(_template);

            this.Model.Contents.Add(_content);
        }

        private void RemoveDefaultImageIfPresent()
        {
            var defaultImg = this.ContentTypesTemplate.FirstOrDefault(v => v.ContentType.ContentName == FILENAME);

            if (defaultImg != null)
            {
                this.ContentTypesTemplate.Remove(defaultImg);

                this.Model.Contents.Remove(defaultImg.ContentType);
            }
        }

        private async Task GetImageSourceFromFile()
        {
            try
            {
                var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(FILENAME);

                if (!_defaulMedia.IsObjectNull())
                {
                    this.ContentTypesTemplate.Clear();

                    var _content = ContentTypeFactory.BuilContentType(_defaulMedia, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID, FILENAME);

                    _content.ContentName = FILENAME;

                    var _template = ContentTypeTemplateFactory.BuilContentType(_content, _content.Content);

                    _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                    this.ContentTypesTemplate.Add(_template);

                    this.Model.Contents.Add(_content);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
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