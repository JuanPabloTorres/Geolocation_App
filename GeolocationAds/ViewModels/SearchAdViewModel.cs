using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class SearchAdViewModel : BaseViewModel2<Advertisement, IGeolocationAdService>
    {
        private readonly IAppSettingService appSettingService;

        private readonly ICaptureService captureService;

        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        public ObservableCollection<string> DistanceSettings { get; set; }

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        public ObservableCollection<NearByTemplateViewModel> NearByTemplateViewModels { get; set; }

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

        public SearchAdViewModel(Advertisement advertisement, ICaptureService captureService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUser) : base(advertisement, geolocationAdService, logUser)
        {
            this.appSettingService = appSettingService;

            this.captureService = captureService;

            this.DistanceSettings = new ObservableCollection<string>();

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.NearByTemplateViewModels = new ObservableCollection<NearByTemplateViewModel>();

            this.SearchCommand = new Command(Initialize);

            this.SelectedAdType = new AppSetting();

            InitializeSettings();

            WeakReferenceMessenger.Default.Register<LogOffMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.NearByTemplateViewModels.Clear();
                });
            });
        }

        private async Task LoadSettings2()
        {
            this.IsLoading = true;

            try
            {
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

            this.IsLoading = false;
        }

        protected override async Task LoadData(object extraData)
        {
            try
            {
                this.NearByTemplateViewModels.Clear();

                var currentLocation = extraData as CurrentLocation;

                var _apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID);

                if (_apiResponse.IsSuccess)
                {
                    if (!_apiResponse.Data.IsObjectNull())
                    {
                        foreach (var item in _apiResponse.Data)
                        {
                            this.NearByTemplateViewModels.Add(new NearByTemplateViewModel(this.captureService, item, this.LogUserPerfilTool));
                        }
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
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
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
            await LoadSettings2();
        }

        //public void Dispose()
        //{
        //    WeakReferenceMessenger.Default.Unregister<LogOffMessage>(this);

        //    // Clear collections or other resources
        //    NearByTemplateViewModels.Clear();

        //    // Optionally, you can also dispose of any disposable objects here if applicable.
        //    // For example, if captureService implements IDisposable:
        //    // captureService.Dispose();

        //    GC.SuppressFinalize(this);
        //}
    }
}