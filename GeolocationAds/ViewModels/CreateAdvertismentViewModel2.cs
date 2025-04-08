using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class CreateAdvertismentViewModel2 : BaseViewModel<Advertisement, IAdvertisementService>
    {
        [ObservableProperty]
        private AppSetting selectedAdType = new();

        [ObservableProperty]
        private string url;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new();

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; } = new();

        private readonly IContainerCreateAdvertisment ContainerCreateAdvertisment;

        public CreateAdvertismentViewModel2(IContainerCreateAdvertisment ContainerCreateAdvertisment) :
            base(ContainerCreateAdvertisment.Model, ContainerCreateAdvertisment.AdvertisementService, ContainerCreateAdvertisment.LogUserPerfilTool)
        {
            this.ContainerCreateAdvertisment = ContainerCreateAdvertisment;

            Task.Run(async () =>
            {
                await LoadSetting();
            });
        }

        public async Task LoadSetting()
        {
            await RunWithLoadingIndicator(async () =>
            {
                this.AdTypesSettings.Clear();

                var apiResponse = await this.ContainerCreateAdvertisment
                .AppSettingService
                .GetAppSettingByName(SettingName.AdTypes.ToString());

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message); // Deja que RunWithLoadingIndicator maneje el error
                }

                AdTypesSettings.AddRange(apiResponse.Data);

                this.SelectedAdType = AdTypesSettings.FirstOrDefault();
            });
        }

        public async Task SetDefault()
        {
            await RunWithLoadingIndicator(async () =>
            {
                this.ContentTypesTemplate.Clear();

                Url = string.Empty;

                SelectedAdType = AdTypesSettings.FirstOrDefault(v => v.Value == AdType.Broadcast.ToString());

                var adSetting = new AdvertisementSettings
                {
                    CreateDate = DateTime.Now,
                    CreateBy = this.LogUserPerfilTool.LogUser.ID,
                    SettingId = this.SelectedAdType.ID,
                };

                this.Model = new Advertisement(this.LogUserPerfilTool.GetUserId(), adSetting);

                await this.GetImageSourceFromFile2();
            },
            onError: async (ex) =>
            {
                if (ex is ArgumentOutOfRangeException)
                {
                    await CommonsTool.DisplayAlert("Index Error", "Please check collection operations.");
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", ex.Message);
                }
            });
        }

        public override async Task Submit(Advertisement obj)
        {
            await RunWithLoadingIndicator(async () =>
            {
                ValidationResults.Clear();

                var validationContextCurrentType = new ValidationContext(obj);

                bool isObjValid = Validator.TryValidateObject(obj, validationContextCurrentType, ValidationResults, true);

                var propertyInstances = ToolsLibrary.Tools.GenericTool<Advertisement>.GetSubPropertiesOfWithForeignKeyAttribute(obj);

                bool allSubPropertyValid = true;

                foreach (var item in propertyInstances)
                {
                    if (!item.IsObjectNull())
                    {
                        var tempValidationResultsSubProperty = new List<ValidationResult>();

                        var validationContextSubProperty = new ValidationContext(item);

                        ValidationContexts.Add(validationContextSubProperty);

                        if (!Validator.TryValidateObject(item, validationContextSubProperty, tempValidationResultsSubProperty, true))
                        {
                            allSubPropertyValid = false;
                        }

                        ValidationResults.AddRange(tempValidationResultsSubProperty);
                    }
                }

                if (isObjValid && allSubPropertyValid)
                {
                    var apiResponse = await this.service.Add(obj);

                    if (!apiResponse.IsSuccess)
                    {
                        throw new Exception(apiResponse.Message);
                    }

                    ValidationResults.Clear();

                    await Shell.Current.CurrentPage.ShowPopupAsync(new CompletePopUp());

                    await this.SetDefault();
                }
            });
        }

        [RelayCommand]
        public async Task UploadContent()
        {
            await RunWithLoadingIndicator(async () =>
            {
                var customFileTypes = GetCommonFileTypes();

                FileResult result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileTypes,
                });

                if (!result.IsObjectNull())
                {
                    this.Model.Contents.Clear();

                    this.ContentTypesTemplate.Clear();

                    await ProcessSelectedFile(result);
                }

                foreach (var item in ContentTypesTemplate)
                {
                    await item.SetAnimation();
                }
            });
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
                    await ProcessImageContent(result);
                    break;

                case ContentVisualType.Video:

                    await ProcessVideoContent(result);

                    break;
            }
        }

        private async Task ProcessImageContent(FileResult result)
        {
            var fileBytes = await CommonsTool.GetFileBytesAsync(result);

            var contentType = CommonsTool.GetContentType(result.FileName);

            if (contentType != ContentVisualType.Image) return;

            var content = ContentTypeFactory.BuilContentType(fileBytes, ContentVisualType.Image, null, this.LogUserPerfilTool.LogUser.ID, result.FileName, result.FullPath);

            var template = ContentTypeTemplateFactory.BuilContentType(content, content.Content);

            this.ContentTypesTemplate.Add(template);

            this.Model.Contents.Add(content);
        }

        private async Task ProcessVideoContent(FileResult result)
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

            //_template.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

            this.ContentTypesTemplate.Add(_template);

            this.Model.Contents.Add(_content);
        }

        private async Task GetImageSourceFromFile2()
        {
            try
            {
                var _defaultTemplate = await AppToolCommon.GetDefaultContentTypeTemplateViewModel(this.LogUserPerfilTool.GetUserId());

                this.ContentTypesTemplate.Add(_defaultTemplate);

                this.Model.Contents.Add(_defaultTemplate.ContentType);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

         async partial void OnSelectedAdTypeChanged(AppSetting value)
        {
            await HandleAdTypeChangeAsync(value);
        }

        [RelayCommand]
        public async Task SetURL(string url)
        {
            await RunWithLoadingIndicator(async () =>
            {
                this.Model.Contents.Clear();

                this.ContentTypesTemplate.Clear();

                if (!CommonsTool.IsValidUrl(url))
                {
                    throw new Exception("URL invalid.");
                }

                var uri = new Uri(url);

                var content = ContentTypeFactory.BuilContentType(url, ContentVisualType.URL, null, this.LogUserPerfilTool.LogUser.ID, null, null);

                var template = ContentTypeTemplateFactory.BuilContentType(content, uri);

                this.ContentTypesTemplate.Add(template);

                this.Model.Contents.Add(content);

                this.Url = string.Empty;
            });
        }

        private async Task HandleAdTypeChangeAsync(AppSetting value)
        {
            await RunWithLoadingIndicator(async () =>
            {
                if (this.Model.Settings.IsObjectNull() || value.IsObjectNull())
                    return;

                this.Model.Settings.Clear(); // 🔹 No es necesario verificar si tiene elementos antes de limpiar

                this.Model.Settings.Add(new AdvertisementSettings
                {
                    CreateDate = DateTime.Now,
                    CreateBy = this.LogUserPerfilTool.LogUser.ID,
                    SettingId = value.ID
                });
            });
        }

        [RelayCommand]
        public async Task Clear()
        {
            await SetDefault();
        }
    }
}