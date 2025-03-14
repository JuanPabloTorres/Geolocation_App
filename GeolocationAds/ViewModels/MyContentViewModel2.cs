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

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new();

        public MyContentViewModel2(IContainerMyContentServices myContentServices) : base(myContentServices.AdLocationTemplateViewModel, myContentServices.GeolocationAdService, myContentServices.LogUserPerfilTool)
        {
            this.containerMyContentServices = myContentServices;

            //BaseTemplateViewModel.ItemDeleted += AdLocationTemplateViewModel_ItemDeleted;

            Task.Run(async () =>
            {
                await InitializeSettingsAsync();

                await InitializeAsync();
            });

            WeakReferenceMessenger.Default.Register<UpdateMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (m?.Value == null) return; // 🔹 Evita errores si el mensaje es nulo

                    foreach (var item in CollectionModel)
                    {
                        if (item.Advertisement.ID == m.Value.ID)
                        {
                            item.Advertisement = m.Value;
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