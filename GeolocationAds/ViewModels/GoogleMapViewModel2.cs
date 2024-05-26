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
    public partial class GoogleMapViewModel2 : BaseViewModel3<Pin, IGeolocationAdService>
    {
        public IList<Pin> FoundLocations = new List<Pin>();

        private readonly IContainerMapServices containerMapServices;

        [ObservableProperty]
        private AppSetting selectedAdType;

        [ObservableProperty]
        private string selectedDistance;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();

        public ObservableCollection<string> DistanceSettings { get; set; } = new ObservableCollection<string>();

        public delegate void PinsUpdatedEventHandler(object sender, EventArgs e);

        public event PinsUpdatedEventHandler PinsUpdated;

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
            try
            {
                this.IsLoading = true;

                var locationReponse = await GeolocationTool.GetLocation();

                if (locationReponse.IsSuccess)
                {
                    var _currentLocation = new CurrentLocation(locationReponse.Data.Latitude, locationReponse.Data.Longitude);

                    var _apiResponse = await this.service.FindAdsNearby(_currentLocation, this.SelectedDistance, SelectedAdType.ID);

                    this.CollectionModel.Clear();

                    if (_apiResponse.IsSuccess)
                    {
                        if (!_apiResponse.Data.IsObjectNull())
                        {
                            foreach (var adsGeoItem in _apiResponse.Data)
                            {
                                Location location = new Location()
                                {
                                    Latitude = adsGeoItem.Latitude,
                                    Longitude = adsGeoItem.Longitude,
                                };

                                this.CollectionModel.Add(new Microsoft.Maui.Controls.Maps.Pin()
                                {
                                    Location = location,
                                    Label = adsGeoItem.Advertisement.Title,
                                    Address = adsGeoItem.Advertisement.Description,
                                    Type = Microsoft.Maui.Controls.Maps.PinType.Generic,
                                });
                            }

                            PinsUpdated.Invoke(this.CollectionModel, null);
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                this.IsLoading = false;
            }
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
            IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

            try
            {
                this.IsLoading = true;

                var _apiResponse = await this.containerMapServices.AppSettingService.GetAppSettingByNames(settings);

                if (_apiResponse.IsSuccess)
                {
                    foreach (var item in _apiResponse.Data)
                    {
                        if (SettingName.MeterDistance.ToString() == item.SettingName)
                        {
                            DistanceSettings.Add(item.Value);
                        }

                        if (SettingName.AdTypes.ToString() == item.SettingName)
                        {
                            AdTypesSettings.Add(item);
                        }
                    }

                    SelectedAdType = AdTypesSettings.FirstOrDefault();

                    SelectedDistance = DistanceSettings.FirstOrDefault();

                    filterPopUpViewModel = new FilterPopUpViewModel2(this.AdTypesSettings, this.DistanceSettings);

                    this.filterPopUpViewModel.OnFilterItem += FilterPopUpViewModel_FilterItem;
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

        [RelayCommand]
        public async Task Search()
        {
            await InitializeAsync();
        }

        private async void FilterPopUpViewModel_FilterItem(object sender, EventArgs e)
        {
            try
            {
                await this._filterPopUpForSearch.CloseAsync();

                PageIndex = 1;

                if (sender is FilterPopUpViewModel2 filterPopUpViewModel)
                {
                    this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                    this.SelectedDistance = filterPopUpViewModel.SelectedDistance;

                    await LoadData();
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}