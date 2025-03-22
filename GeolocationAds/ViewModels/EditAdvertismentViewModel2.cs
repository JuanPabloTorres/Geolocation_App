using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.AppTools;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class EditAdvertismentViewModel2 : BaseViewModel3<Advertisement, IAdvertisementService>
    {
        private readonly IContainerEditAdvertisment containerEditAdvertisment;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new();

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; } = new();

        [ObservableProperty]
        private AppSetting selectedAdType = new();

        [ObservableProperty]
        private string url;

        public EditAdvertismentViewModel2(IContainerEditAdvertisment containerEditAdvertisment) : base(containerEditAdvertisment.Model, containerEditAdvertisment.AdvertisementService, containerEditAdvertisment.LogUserPerfilTool)
        {
            this.containerEditAdvertisment = containerEditAdvertisment;

            // Asigna la acción a ejecutar cuando ApplyQueryAttributesCompleted sea invocado
            this.ApplyQueryAttributesCompleted = async () => await EditAdvertismentViewModel_ApplyQueryAttributesCompleted();
        }

        private async Task EditAdvertismentViewModel_ApplyQueryAttributesCompleted()
        {
            await RunWithLoadingIndicator(async () =>
            {
                // 🔹 Validación previa de Model.Settings para evitar excepciones
                var currentModelAdType = this.Model.Settings?.FirstOrDefault();

                await LoadSetting();

                // 🔹 Asignación segura del tipo de anuncio seleccionado
                this.SelectedAdType = currentModelAdType != null
                    ? AdTypesSettings.FirstOrDefault(item => item.ID == currentModelAdType.SettingId)
                    : AdTypesSettings.FirstOrDefault();

                this.ContentTypesTemplate.Clear();

                // 🔹 Validación segura de Model.Contents antes de procesar
                if (this.Model.Contents?.Any() == true)
                {
                    var contentProcessingTasks = this.Model.Contents
                        .Select(async item =>
                        {
                            var serviceToUse = item.Type == ContentVisualType.Video ? this.service : null;
                            return await AppToolCommon.ProcessContentItem(item, serviceToUse);
                        });

                    // 🔹 Espera la ejecución en paralelo de todas las tareas antes de añadirlas a
                    // la colección
                    var processedTemplates = await Task.WhenAll(contentProcessingTasks);

                    // 🔹 Usa AddRange para evitar múltiples llamadas a la UI
                    this.ContentTypesTemplate.AddRange(processedTemplates);
                }
            });
        }

        public async Task LoadSetting()
        {
            await RunWithLoadingIndicator(async () =>
            {
                this.AdTypesSettings.Clear();

                var apiResponse = await this.containerEditAdvertisment.AppSettingService.GetAppSettingByName(SettingName.AdTypes.ToString());

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message); // Deja que RunWithLoadingIndicator maneje el error
                }

                AdTypesSettings.AddRange(apiResponse.Data);

                this.SelectedAdType = AdTypesSettings.FirstOrDefault();
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
        public async Task GoManageLocation(int id)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var navigationParameter = new Dictionary<string, object> { { "ID", id } };

                await NavigationTool.NavigateAsync(nameof(ManageLocation), navigationParameter);
            });
        }

        [RelayCommand]
        public async Task SetURL(string url)
        {
            //try
            //{
            //    this.IsLoading = true;

            // if (this.Model.Contents.Count == ConstantsTools.MaxAdLimit) { await
            // CommonsTool.DisplayAlert("Limit Reached", "You have reached the maximum content limit permitted.");

            // return; }

            // var _isValidUrl = CommonsTool.IsValidUrl(url);

            // if (_isValidUrl) { var _uri = new Uri(url);

            // var _content = ContentTypeFactory.BuilContentType(url, ContentVisualType.URL, null,
            // this.LogUserPerfilTool.LogUser.ID, null, null);

            // var _template = ContentTypeTemplateFactory.BuilContentType(_content, _uri);

            // //_template.ItemDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

            // _template.ItemDeleted = ContentTypeTemplateViewModel_ContentTypeDeleted;

            // this.ContentTypesTemplate.Add(_template);

            // this.Model.Contents.Add(_content);

            //        this.Url = string.Empty;
            //    }
            //    else
            //    {
            //        await CommonsTool.DisplayAlert("Error", "Url invalid.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await CommonsTool.DisplayAlert("Error", ex.Message);
            //}
            //finally
            //{
            //    this.IsLoading = false;
            //}

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

                //template.ItemDeleted = ContentTypeTemplateViewModel_ContentTypeDeleted;

                this.ContentTypesTemplate.Add(template);

                this.Model.Contents.Add(content);

                this.Url = string.Empty;
            });
        }
    }
}