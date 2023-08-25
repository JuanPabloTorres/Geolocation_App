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

        public SearchAdViewModel(Advertisement advertisement, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService) : base(advertisement, geolocationAdService)
        {
            this.appSettingService = appSettingService;

            this.DistanceSettings = new ObservableCollection<string>();

            this.SearchCommand = new Command(Initialize);

            Task.Run(async () => { await this.LoadSetting(); });

            WeakReferenceMessenger.Default.Register<LogOffMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.CollectionModel.Clear();
                });
            });
        }

        private async Task LoadSetting()
        {
            var _apiResponse = await this.appSettingService.GetAppSettingByName(SettingsEnums.SettingName.ToString());

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
        }

        protected override async Task LoadData(object extraData)
        {
            var currentLocation = extraData as CurrentLocation;

            var _apiResponse = await this.service.FindAdNear(currentLocation, SelectedDistance);

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
    }
}