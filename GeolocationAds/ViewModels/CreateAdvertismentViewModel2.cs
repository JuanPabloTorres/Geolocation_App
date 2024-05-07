using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class CreateAdvertismentViewModel2 : BaseViewModel3<Advertisement, IAdvertisementService>
    {
        [ObservableProperty]
        private AppSetting selectedAdType = new AppSetting();

        private readonly IAppSettingService appSettingService;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting> { };

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; } = new ObservableCollection<ContentTypeTemplateViewModel2> { };

        public CreateAdvertismentViewModel2(Advertisement advertisement, IAdvertisementService advertisementService, LogUserPerfilTool logUserPerfilTool, IAppSettingService appSettingService) : base(advertisement, advertisementService, logUserPerfilTool)
        {
            this.appSettingService = appSettingService;

            WeakReferenceMessenger.Default.Register<CleanOnSubmitMessage<Advertisement>>(this, async (r, m) =>
            {
                this.SetDefault();
            });
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

        [RelayCommand]
        public async Task UploadContent()
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
            try
            {
                var fileBytes = await CommonsTool.GetFileBytesAsync(result);

                var contentType = CommonsTool.GetContentType(result.FileName);

                switch (contentType)
                {
                    case ContentVisualType.Image:
                        await ProcessImageContent(result);
                        break;

                    case ContentVisualType.Video:

                        await ProcessVideoContent(result);

                        break;
                }

                RemoveDefaultImageIfPresent();
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private async void ContentTypeTemplateViewModel_ContentTypeDeleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                if (sender is ContentTypeTemplateViewModel2 template)
                {
                    if (this.ContentTypesTemplate.Count() == 1)
                    {
                        await CommonsTool.DisplayAlert("Error", "At least one item is required.You may remove any excess items.");
                    }
                    else
                    {
                        this.ContentTypesTemplate.Remove(template);

                        this.Model.Contents.Remove(template.ContentType);
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

        private async Task ProcessImageContent(FileResult result)
        {
            try
            {
                var _fileBytes = await CommonsTool.GetFileBytesAsync(result);

                var _contentType = CommonsTool.GetContentType(result.FileName);

                if (_contentType == ContentVisualType.Image)
                {
                    var _content = ContentTypeFactory.BuilContentType(_fileBytes, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID, result.FileName, result.FullPath);

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

        private async Task ProcessVideoContent(FileResult result)
        {
            try
            {
                var _fileBytes = await CommonsTool.GetFileBytesAsync(result);

                if (_fileBytes.Length > ConstantsTools.MaxFileSize)
                {
                    await CommonsTool.DisplayAlert("Error", "File Size is to heavy.");

                    return;
                }

                var _contentType = CommonsTool.GetContentType(result.FileName);

                var _content = ContentTypeFactory.BuilContentType(_fileBytes, ContentVisualType.Video, null, this.LogUserPerfilTool.LogUser.ID, result.FileName, result.FullPath);

                var _file = CommonsTool.SaveByteArrayToTempFile(_fileBytes);

                var _template = ContentTypeTemplateFactory.BuilContentType(_content, result.FullPath);

                _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                this.ContentTypesTemplate.Add(_template);

                this.Model.Contents.Add(_content);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private void RemoveDefaultImageIfPresent()
        {
            var defaultImg = this.ContentTypesTemplate.FirstOrDefault(v => v.ContentType.ContentName == ConstantsTools.FILENAME);

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
                var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(ConstantsTools.FILENAME);

                if (!_defaulMedia.IsObjectNull())
                {
                    this.ContentTypesTemplate.Clear();

                    var _content = ContentTypeFactory.BuilContentType(_defaulMedia, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID, ConstantsTools.FILENAME);

                    _content.ContentName = ConstantsTools.FILENAME;

                    _content.FilePath = AppToolCommon.EnsureImageFile(ConstantsTools.FILENAME);

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

        private async Task GetImageSourceFromFile2()
        {
            try
            {
                var _defaultTemplate = await AppToolCommon.GetDefaultContentTypeTemplateViewModel(this.LogUserPerfilTool.GetUserId());

                _defaultTemplate.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                this.ContentTypesTemplate.Add(_defaultTemplate);

                this.Model.Contents.Add(_defaultTemplate.ContentType);

                //var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(ConstantsTools.FILENAME);

                //if (!_defaulMedia.IsObjectNull())
                //{
                //    this.ContentTypesTemplate.Clear();

                //    var _content = ContentTypeFactory.BuilContentType(_defaulMedia, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID, ConstantsTools.FILENAME);

                //    _content.ContentName = ConstantsTools.FILENAME;

                //    var _template = ContentTypeTemplateFactory.BuilContentType(_content, _content.Content);

                //    _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                //    this.ContentTypesTemplate.Add(_template);

                //    this.Model.Contents.Add(_content);
                //}
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