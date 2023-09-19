using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class SearchAdViewModel : BaseViewModel2<Advertisement, IGeolocationAdService>
    {
        private IAppSettingService appSettingService;

        private ObservableCollection<string> _distanceSettings;

        public ObservableCollection<string> DistanceSettings
        {
            get => _distanceSettings;
            set
            {
                if (_distanceSettings != value)
                {
                    _distanceSettings = value;

                    OnPropertyChanged();
                }
            }
        }

        private string _selectedDistance;

        public string SelectedDistance
        {
            get => _selectedDistance;
            set
            {
                if (_selectedDistance != value)
                {
                    _selectedDistance = value;

                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<AppSetting> _adTypesSettings;

        public ObservableCollection<AppSetting> AdTypesSettings
        {
            get => _adTypesSettings;
            set
            {
                if (_adTypesSettings != value)
                {
                    _adTypesSettings = value;

                    OnPropertyChanged();
                }
            }
        }

        private AppSetting _selectedAdType;

        public AppSetting SelectedAdType
        {
            get => _selectedAdType;
            set
            {
                if (_selectedAdType != value)
                {
                    _selectedAdType = value;

                    OnPropertyChanged();
                }
            }
        }

        public SearchAdViewModel(Advertisement advertisement, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService) : base(advertisement, geolocationAdService)
        {
            this.appSettingService = appSettingService;

            this.DistanceSettings = new ObservableCollection<string>();

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.SearchCommand = new Command(Initialize);

            this.SelectedAdType = new AppSetting();

            InitializeSettings();

            WeakReferenceMessenger.Default.Register<LogOffMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.CollectionModel.Clear();
                });
            });
        }

        private async Task LoadSettings()
        {
            var _apiResponse = await this.appSettingService.GetAppSettingByName(SettingName.MeterDistance.ToString());

            this.CollectionModel.Clear();

            if (_apiResponse.IsSuccess)
            {
                foreach (var item in _apiResponse.Data)
                {
                    DistanceSettings.Add(item.Value);
                }

                SelectedDistance = DistanceSettings.FirstOrDefault();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }

            var _typeSettingResponse = await this.appSettingService.GetAppSettingByName(SettingName.AdTypes.ToString());

            if (_typeSettingResponse.IsSuccess)
            {
                foreach (var item in _typeSettingResponse.Data)
                {
                    AdTypesSettings.Add(item);
                }

                SelectedAdType = AdTypesSettings.FirstOrDefault();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }
        }

        private async Task LoadSettings2()
        {
            try
            {
                this.CollectionModel.Clear();

                IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

                var _apiResponse = await this.appSettingService.GetAppSettingByNames(settings);

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
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override async Task LoadData(object extraData)
        {
            var currentLocation = extraData as CurrentLocation;

            var _apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID);

            this.CollectionModel.Clear();

            if (_apiResponse.IsSuccess)
            {
                if (!_apiResponse.Data.IsObjectNull())
                {
                    this.CollectionModel.AddRange(_apiResponse.Data);
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

        public async void Initialize()
        {
            this.IsLoading = true;

            var locationReponse = await GeolocationTool.GetLocation();

            if (locationReponse.IsSuccess)
            {
                var _currentLocation = new CurrentLocation(locationReponse.Data.Latitude, locationReponse.Data.Longitude);

                await LoadData(_currentLocation);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
            }

            this.IsLoading = false;
        }

        public async void InitializeSettings()
        {
            this.IsLoading = true;

            await LoadSettings2();

            this.IsLoading = false;
        }
    }
}