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
    public partial class CreateAdvertismentViewModel2 : BaseViewModel3<Advertisement, IAdvertisementService>
    {
        [ObservableProperty]
        private AppSetting selectedAdType = new AppSetting();

        [ObservableProperty]
        private string url;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; } = new ObservableCollection<ContentTypeTemplateViewModel2>();

        private readonly IContainerCreateAdvertisment ContainerCreateAdvertisment;

        public CreateAdvertismentViewModel2(IContainerCreateAdvertisment ContainerCreateAdvertisment) : base(ContainerCreateAdvertisment.Model, ContainerCreateAdvertisment.AdvertisementService, ContainerCreateAdvertisment.LogUserPerfilTool)
        {
            this.ContainerCreateAdvertisment = ContainerCreateAdvertisment;

            //ContentTypeTemplateViewModel2.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

            Task.Run(async () =>
            {
                await DataInitialization();
            });

            //WeakReferenceMessenger.Default.Register<CleanOnSubmitMessage<Advertisement>>(this, async (r, m) =>
            //{
            //    await this.SetDefault();
            //});

            // Subscribe to the ItemDeletedEvent
            EventManager2.Instance.Subscribe<ContentTypeTemplateViewModel2>(async (eventArgs) => { await HandleItemDeletedEventAsync(eventArgs); }, this);
        }

        private async Task HandleItemDeletedEventAsync(ContentTypeTemplateViewModel2 eventArgs)
        {
            try
            {
                this.IsLoading = true;

                if (this.ContentTypesTemplate.Count() == 1)
                {
                    await CommonsTool.DisplayAlert("Error", "At least one item is required.You may remove any excess items.");
                }
                else
                {
                    this.ContentTypesTemplate.Remove(eventArgs);

                    foreach (var item in ContentTypesTemplate)
                    {
                        await item.SetAnimation();
                    }

                    this.Model.Contents.Remove(eventArgs.ContentType);
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

        public async Task DataInitialization()
        {
            await InitializeSettings();

            //await SetDefault();
        }

        public async Task LoadSetting()
        {
            try
            {
                this.IsLoading = true;

                this.AdTypesSettings.Clear();

                var _apiResponse = await this.ContainerCreateAdvertisment.AppSettingService.GetAppSettingByName(SettingName.AdTypes.ToString());

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

        public async Task SetDefault()
        {
            try
            {
                this.IsLoading = true;

                ContentTypeTemplateViewModel2.CurrentPageContext = this.GetType().Name;

                if (this.ContentTypesTemplate.Any())
                {
                    this.ContentTypesTemplate.Clear();
                }

                var _adSetting = new AdvertisementSettings()
                {
                    CreateDate = DateTime.Now,
                    CreateBy = this.LogUserPerfilTool.LogUser.ID,
                    SettingId = this.SelectedAdType.ID,
                };

                this.Model = new Advertisement(this.LogUserPerfilTool.GetUserId(), _adSetting);

                await this.GetImageSourceFromFile2();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Específico para el error de índice fuera de rango
                await CommonsTool.DisplayAlert("Index Error", "Please check collection operations.");
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
        public override async Task Submit(Advertisement obj)
        {
            try
            {
                IsLoading = true;

                if (ValidationResults.Any())
                {
                    ValidationResults.Clear();
                }

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

                    if (apiResponse.IsSuccess)
                    {
                        if (ValidationResults.Any())
                        {
                            ValidationResults.Clear();
                        }

                        IsLoading = false;

                        await Shell.Current.CurrentPage.ShowPopupAsync(new CompletePopUp());

                        await this.SetDefault();

                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", apiResponse.Message, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                IsLoading = false;
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

                        foreach (var item in ContentTypesTemplate)
                        {
                            await item.SetAnimation();
                        }

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

                    _template.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

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

                _template.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

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

        private void RemoveDefaultImageIfPresent()
        {
            if (this.ContentTypesTemplate.Count > 0)
            {
                var defaultImg = this.ContentTypesTemplate.FirstOrDefault(v => v.ContentType.ContentName == ConstantsTools.FILENAME);

                if (defaultImg != null)
                {
                    this.ContentTypesTemplate.Remove(defaultImg);

                    this.Model.Contents.Remove(defaultImg.ContentType);
                }
            }
        }

        private async Task GetImageSourceFromFile2()
        {
            try
            {
                var _defaultTemplate = await AppToolCommon.GetDefaultContentTypeTemplateViewModel(this.LogUserPerfilTool.GetUserId());

                _defaultTemplate.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                this.ContentTypesTemplate.Add(_defaultTemplate);

                this.Model.Contents.Add(_defaultTemplate.ContentType);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
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

        [RelayCommand]
        public async Task SetURL(string url)
        {
            try
            {
                this.IsLoading = true;

                if (this.Model.Contents.Count == 3)
                {
                    await CommonsTool.DisplayAlert("Limit Reached", "You have reached the maximum content limit permitted.");

                    return;
                }

                var _isValidUrl = CommonsTool.IsValidUrl(url);

                if (_isValidUrl)
                {
                    RemoveDefaultImageIfPresent();

                    var _uri = new Uri(url);

                    var _content = ContentTypeFactory.BuilContentType(url, ContentVisualType.URL, null, this.LogUserPerfilTool.LogUser.ID, null, null);

                    var _template = ContentTypeTemplateFactory.BuilContentType(_content, _uri);

                    _template.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                    this.ContentTypesTemplate.Add(_template);

                    this.Model.Contents.Add(_content);

                    this.Url = string.Empty;
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", "Url invalid.");
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