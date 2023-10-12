using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
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
    public partial class EditAdvertismentViewModel : BaseViewModel2<Advertisement, IAdvertisementService>
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

                    SelectedTypeChange(SelectedAdType);

                    OnPropertyChanged();
                }
            }
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
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private IAppSettingService appSettingService;

        public ICommand UploadContentCommand { get; set; }

        public EditAdvertismentViewModel(Advertisement model, IAdvertisementService service, LogUserPerfilTool logUserPerfil, IAppSettingService appSettingService) : base(model, service, logUserPerfil)
        {
            this.appSettingService = appSettingService;

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            this.SelectedAdType = new AppSetting();

            UploadContentCommand = new Command(OnUploadCommandExecuted2);

            this.ApplyQueryAttributesCompleted += EditAdvertismentViewModel_ApplyQueryAttributesCompleted;

            ContentTypeTemplateViewModel.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;
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

                if (sender is ContentTypeTemplateViewModel template)
                {
                    if (!template.IsObjectNull())
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

        private async void EditAdvertismentViewModel_ApplyQueryAttributesCompleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                this.ContentTypesTemplate.CollectionChanged -= ContentTypes_CollectionChanged;

                await this.LoadSetting();

                foreach (var item in this.Model.Contents)
                {
                    var _template = await AppToolCommon.ProcessContentItem(item);

                    this.ContentTypesTemplate.Add(_template);
                }

                this.ContentTypesTemplate.CollectionChanged += ContentTypes_CollectionChanged;
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

                    //foreach (var item in AdTypesSettings)
                    //{
                    //    if (this.Model.Settings.IsNotNullOrCountGreaterZero())
                    //    {
                    //        foreach (var modelsetting in this.Model.Settings)
                    //        {
                    //            if (modelsetting.SettingId == item.ID)
                    //            {
                    //                this.SelectedAdType = item;

                    //                return;
                    //            }
                    //        }

                    //        //this.SelectedAdType = this.Model.Settings
                    //        //    .Where(v => v.SettingId == item.ID)
                    //        //    .Select(s =>
                    //        //    new AppSetting()
                    //        //    {
                    //        //        ID = s.SettingId
                    //        //    }).FirstOrDefault();
                    //    }
                    //}

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

                        //this.Model.Contents.Add(_content);

                        var _template = ContentTypeTemplateFactory.BuilContentType(_content);

                        this.ContentTypesTemplate.Add(_template);
                    }
                    else
                    {
                        var _content = ContentTypeFactory.BuilContentType(fileBytes, ContentVisualType.Video, null, this.LogUserPerfilTool.LogUser.ID);

                        //this.Model.Contents.Add(_content);

                        var _template = ContentTypeTemplateFactory.BuilContentType(_content, result.FullPath);

                        this.ContentTypesTemplate.Add(_template);
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