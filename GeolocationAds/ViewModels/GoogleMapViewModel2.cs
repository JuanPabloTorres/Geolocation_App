using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.Tools;
using Microsoft.Maui.Controls.Maps;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class GoogleMapViewModel2 : BaseViewModel<Pin, IGeolocationAdService>
    {
        public IList<Pin> FoundLocations = new List<Pin>();

        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        private readonly IContainerMapServices containerMapServices;

        [ObservableProperty]
        private AppSetting selectedAdType;

        [ObservableProperty]
        private string selectedDistance;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new();

        public ObservableCollection<string> DistanceSettings { get; set; } = new();

        public Action<ObservableCollection<Pin>>? PinsUpdated { get; set; }

        public GoogleMapViewModel2(IContainerMapServices containerMapServices) : base(containerMapServices.Model, containerMapServices.GeolocationAdService, containerMapServices.LogUserPerfilTool)
        {
            this.containerMapServices = containerMapServices;

            Task.Run(async () =>
            {
                await InitializeSettingsAsync();
            });
        }

        protected override async Task LoadData(int? pageIndex = 1)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var locationResponse = await GeolocationTool.GetLocation();

                if (!locationResponse.IsSuccess)
                    throw new Exception(locationResponse.Message);

                var currentLocation = new CurrentLocation(locationResponse.Data.Latitude, locationResponse.Data.Longitude);

                var apiResponse = await service.FindAdsNearby(currentLocation, SelectedDistance, SelectedAdType.ID);

                CollectionModel.Clear();

                if (!apiResponse.IsSuccess)
                {
                    PinsUpdated?.Invoke(CollectionModel);

                    throw new Exception(apiResponse.Message);
                }

                if (apiResponse.Data.IsObjectNull())
                {
                    PinsUpdated?.Invoke(CollectionModel);

                    throw new Exception("No nearby ads found.");
                }

                foreach (var adsGeoItem in apiResponse.Data)
                {
                    var location = new Location(adsGeoItem.Latitude, adsGeoItem.Longitude);

                    CollectionModel.Add(new Microsoft.Maui.Controls.Maps.Pin
                    {
                        Location = location,
                        Label = adsGeoItem.Advertisement.Title,
                        Address = adsGeoItem.Advertisement.Description,
                        Type = Microsoft.Maui.Controls.Maps.PinType.Generic
                    });
                }

                PinsUpdated?.Invoke(CollectionModel);
            });
        }

        public async Task InitializeAsync()
        {
            await this.LoadData();
        }

        public IEnumerable<Pin> GetContentPins()
        {
            return this.CollectionModel.ToList();
        }

        public async Task InitializeSettingsAsync()
        {
            await LoadSettings2Async();
        }

        private async Task LoadSettings2Async()
        {
            await RunWithLoadingIndicator(async () =>
            {
                var _apiResponse = await containerMapServices.AppSettingService.GetAppSettingByNames(settings);

                if (!_apiResponse.IsSuccess)
                    throw new Exception(_apiResponse.Message);

                foreach (var item in _apiResponse.Data)
                {
                    if (item.SettingName == SettingName.MeterDistance.ToString())
                        DistanceSettings.Add(item.Value);

                    if (item.SettingName == SettingName.AdTypes.ToString())
                        AdTypesSettings.Add(item);
                }

                SelectedAdType = AdTypesSettings.FirstOrDefault();

                SelectedDistance = DistanceSettings.FirstOrDefault();

                filterPopUpViewModel = new FilterPopUpViewModel2(AdTypesSettings, DistanceSettings);

                filterPopUpViewModel.OnFilterItem = FilterPopUpViewModel_FilterItem;
            });
        }

        [RelayCommand]
        public async Task Search()
        {
            await InitializeAsync();
        }

        private async void FilterPopUpViewModel_FilterItem()
        {
            try
            {
                await this._filterPopUpForSearch.CloseAsync();

                PageIndex = 1;

                this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                this.SelectedDistance = filterPopUpViewModel.SelectedDistance;

                await LoadData();
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}