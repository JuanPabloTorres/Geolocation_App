using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class MyContentViewModel2 : BaseViewModel3<ContentViewTemplateViewModel, IGeolocationAdService>
    {
        private readonly IContainerMyContentServices containerMyContentServices;

        [ObservableProperty]
        private AppSetting selectedAdType;

        [ObservableProperty]
        private string isResetMessage = "No content available.";

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new();

        public MyContentViewModel2(IContainerMyContentServices myContentServices) : base(myContentServices.AdLocationTemplateViewModel, myContentServices.GeolocationAdService, myContentServices.LogUserPerfilTool)
        {
            this.containerMyContentServices = myContentServices;

            Task.Run(async () =>
            {
                await InitializeSettingsAsync();

                await InitializeAsync();
            });

            WeakReferenceMessenger.Default.Register<UpdateMessage<Advertisement>>(this, (r, m) =>
            {
                if (m?.Value == null) return; // 🔹 Evita errores si el mensaje es nulo

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var existingItem = CollectionModel.FirstOrDefault(x => x.Advertisement.ID == m.Value.ID);

                    if (existingItem != null)
                    {
                        var existingType = existingItem.Advertisement.Contents.FirstOrDefault()?.Type;

                        var newType = m.Value.Contents.FirstOrDefault()?.Type;

                        var existingSettingId = existingItem.Advertisement.Settings.FirstOrDefault()?.ID;

                        var newSettingId = SelectedAdType?.ID;

                        if (existingType == newType && existingSettingId == newSettingId)
                        {
                            // 🔹 Sustituye el elemento actualizado en la misma posición
                            var itemIndex = CollectionModel.IndexOf(existingItem);

                            CollectionModel[itemIndex] = new ContentViewTemplateViewModel(
                                containerMyContentServices.AdvertisementService,
                                service,
                                m.Value,
                                On_ItemDeleted
                            );
                        }
                        else
                        {
                            // 🔹 Elimina el elemento si el tipo de contenido o el Setting ID no coinciden
                            CollectionModel.Remove(existingItem);
                        }
                    }
                });
            });
        }

        [RelayCommand]
        public async Task Search()
        {
            PageIndex = 1;

            this.CollectionModel.Clear();

            await InitializeAsync();
        }

        private async void FilterPopUpViewModel_FilterItem()
        {
            try
            {
                await this._filterPopUp.CloseAsync();

                this.CollectionModel.Clear();

                this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                await InitializeAsync();
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private void On_ItemDeleted(ContentViewTemplateViewModel sender)
        {
            var _toRemoveAdContent = this.CollectionModel.Where(v => v.Advertisement.ID == sender.Advertisement.ID).FirstOrDefault();

            this.CollectionModel.Remove(_toRemoveAdContent);
        }

        protected override async Task LoadData(int? pageIndex = 1)
        {
            await RunWithLoadingIndicator(async () =>
            {
                IsResetMessage = ""; // 🔹 Oculta el mensaje mientras carga

                var userId = LogUserPerfilTool.GetUserId();

                var apiResponse = await containerMyContentServices.AdvertisementService
                    .GetAdvertisementsOfUser(userId, SelectedAdType?.ID ?? 0, pageIndex)
                    .ConfigureAwait(false);

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message);
                }

                var newViewModels = apiResponse.Data
                    .AsParallel() // 🔹 Usa procesamiento paralelo para mayor eficiencia
                    .Select(ad => new ContentViewTemplateViewModel(containerMyContentServices.AdvertisementService, service, ad, On_ItemDeleted))
                    .ToList();

                // 🔹 Modifica la UI solo si hay cambios y en el hilo principal
                if (newViewModels.Count > 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        //CollectionModel.Clear(); // 🔹 Limpia antes de agregar nuevos elementos
                        CollectionModel.AddRange(newViewModels);
                    });
                }

                // 🔹 Si la lista está vacía después de cargar, vuelve a mostrar el mensaje
                IsResetMessage = CollectionModel.Count == 0 ? "No content available." : "";
            });
        }

        private async Task LoadSettingsAsync()
        {
            IList<string> settings = new List<string> { SettingName.AdTypes.ToString() };

            await RunWithLoadingIndicator(async () =>
            {
                AdTypesSettings.Clear();

                var apiResponse = await containerMyContentServices.AppSettingService.GetAppSettingByNames(settings);

                if (!apiResponse.IsSuccess)
                {
                    await CommonsTool.DisplayAlert("Error", apiResponse.Message);

                    return;
                }

                // 🔹 Filtra y agrega los elementos de forma más eficiente
                AdTypesSettings.AddRange(apiResponse.Data.Where(item => item.SettingName == SettingName.AdTypes.ToString()));

                SelectedAdType = AdTypesSettings.FirstOrDefault();

                filterPopUpViewModel = new FilterPopUpViewModel2(AdTypesSettings);

                filterPopUpViewModel.OnFilterItem = FilterPopUpViewModel_FilterItem;
            });
        }

        public async Task InitializeSettingsAsync()
        {
            await LoadSettingsAsync();
        }

        public async Task InitializeAsync(int? pageIndex = 1)
        {
            await LoadData(pageIndex);
        }

        public override async Task OpenFilterPopUpForSearch()
        {
            await RunWithLoadingIndicator(async () =>
            {
                _filterPopUp = new FilterPopUp(filterPopUpViewModel);

                await Shell.Current.CurrentPage.ShowPopupAsync(_filterPopUp);
            });
        }
    }
}