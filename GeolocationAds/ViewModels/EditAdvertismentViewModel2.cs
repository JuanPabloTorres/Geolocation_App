using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
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

            ContentTypeTemplateViewModel2.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

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

                foreach (var item in ContentTypesTemplate.Where(v => v.ContentVisualType == ContentVisualType.Image))
                {
                    await item.SetAnimation();
                }

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
        public async Task UploadContent()
        {
            try
            {
                this.IsLoading = true;

                if (this.Model.Contents.Count == 3)
                {
                    await CommonsTool.DisplayAlert("Limit Reached", "You have reached the maximum content limit permitted.");

                    return;
                }

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
                //ContentTypesTemplate.Select(async v => await v.SetAnimation());

                foreach (var item in ContentTypesTemplate)
                {
                    await item.SetAnimation();
                }

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


            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
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

                if (_fileBytes.Length > ConstantsTools.MB50)
                {
                    await CommonsTool.DisplayAlert("Error", "File Size is to heavy.");

                    return;
                }

                var _contentType = CommonsTool.GetContentType(result.FileName);

                var _content = ContentTypeFactory.BuilContentType(_fileBytes, ContentVisualType.Video, null, this.LogUserPerfilTool.LogUser.ID, result.FileName, result.FullPath);

                var _file = CommonsTool.SaveByteArrayToTempFile(_fileBytes);

                var _template = ContentTypeTemplateFactory.BuilContentType(_content, result.FullPath);

                this.ContentTypesTemplate.Add(_template);

                this.Model.Contents.Add(_content);
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

        partial void OnSelectedAdTypeChanged(AppSetting value)
        {
            // Invoke the asynchronous method and forget it
            HandleAdTypeChangeAsync(value).ContinueWith(task =>
            {
                // Handle exceptions if task fails
                if (task.Exception != null)
                {
                    // Log or handle the exception as needed
                    Console.WriteLine($"Exception occurred: {task.Exception.Flatten()}");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext()); // Ensure any continuation runs on the UI thread
        }

        private async Task HandleAdTypeChangeAsync(AppSetting value)
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
                // If DisplayAlert is truly asynchronous
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}