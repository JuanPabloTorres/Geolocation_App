using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
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
    public partial class EditAdvertismentViewModel2 : BaseViewModel3<Advertisement, IAdvertisementService>
    {
        private byte[] fileBytes;

        private IAppSettingService appSettingService;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; } = new ObservableCollection<ContentTypeTemplateViewModel2>();

        [ObservableProperty]
        private AppSetting selectedAdType = new AppSetting();

        public EditAdvertismentViewModel2(Advertisement model, IAdvertisementService service, LogUserPerfilTool logUserPerfil, IAppSettingService appSettingService) : base(model, service, logUserPerfil)
        {
            this.appSettingService = appSettingService;

            this.ApplyQueryAttributesCompleted += EditAdvertismentViewModel_ApplyQueryAttributesCompleted;
        }

        private async void ContentTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (sender is IList<ContentTypeTemplateViewModel> contents)
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

        private async void ContentTypeTemplateViewModel_ContentTypeDeleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                if (sender is ContentTypeTemplateViewModel2 template)
                {
                    this.ContentTypesTemplate.Remove(template);

                    this.Model.Contents.Remove(this.Model.Contents.Where(v => v.ID == template.ContentType.ID).FirstOrDefault());
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

        private async void EditAdvertismentViewModel_ApplyQueryAttributesCompleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                await this.LoadSetting();

                foreach (var item in this.Model.Contents)
                {
                    var _template = await AppToolCommon.ProcessContentItem(item);

                    _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                    this.ContentTypesTemplate.Add(_template);
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

        public async Task LoadSetting()
        {
            try
            {
                var _apiResponse = await this.appSettingService.GetAppSettingByName(SettingName.AdTypes.ToString());

                if (_apiResponse.IsSuccess)
                {
                    AdTypesSettings.AddRange(_apiResponse.Data);

                    var matchingSetting = this.Model.Settings.FirstOrDefault(modelsetting => AdTypesSettings.Any(item => modelsetting.SettingId == item.ID));

                    if (!matchingSetting.IsObjectNull())
                    {
                        this.SelectedAdType = AdTypesSettings.First(item => matchingSetting.SettingId == item.ID);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        [RelayCommand]
        private async Task UploadContent()
        {
            try
            {
                this.IsLoading = true;

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

                    if (_contentType == ContentVisualType.Video)
                    {
                        var _content = ContentTypeFactory.BuilContentType(_fileBytes, ContentVisualType.Video, null, this.LogUserPerfilTool.LogUser.ID, result.FileName);

                        var _file = CommonsTool.SaveByteArrayToTempFile(_fileBytes);

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
    }
}