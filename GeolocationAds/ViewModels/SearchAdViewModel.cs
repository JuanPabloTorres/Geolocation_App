using CommunityToolkit.Maui.Views;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
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

        private FilterPopUpViewModel filterPopUpViewModel;

        public SearchAdViewModel(Advertisement advertisement, ICaptureService captureService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUser) : base(advertisement, geolocationAdService, logUser)
        {
            this.appSettingService = appSettingService;

            this.captureService = captureService;

            this.DistanceSettings = new ObservableCollection<string>();

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.NearByTemplateViewModels = new ObservableCollection<NearByTemplateViewModel>();

            this.SearchCommand = new Command(async () => await InitializeAsync());

            this.OpenFilterPopUpCommand = new Command(OpenFilterPopUp);

            this.SelectedAdType = new AppSetting();

            //this.service.SetJwtToken(this.LogUserPerfilTool.JsonToken);

            InitializeSettingsAsync();

            //WeakReferenceMessenger.Default.Register<LogOffMessage>(this, (r, m) =>
            //{
            //    MainThread.BeginInvokeOnMainThread(() =>
            //    {
            //        this.NearByTemplateViewModels.Clear();
            //    });
            //});
        }

        private async void FilterPopUpViewModel_FilterItem(object sender, EventArgs e)
        {
            try
            {
                await this._filterPopUpForSearch.CloseAsync();

                if (sender is FilterPopUpViewModel filterPopUpViewModel)
                {
                    this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                    this.SelectedDistance = filterPopUpViewModel.SelectedDistance;

                    await InitializeAsync();
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private async Task LoadSettings2Async()
        {
            try
            {
                this.IsLoading = true;

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

                    filterPopUpViewModel = new FilterPopUpViewModel(this.AdTypesSettings, this.DistanceSettings);

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

        protected override async Task LoadData(object extraData)
        {
            try
            {
                if (!(extraData is CurrentLocation currentLocation))
                {
                    // Handle invalid 'extraData' if needed.
                    await Shell.Current.DisplayAlert("Error", "Failed to retrieve location information. Please check your settings and try again.", "OK");

                    return;
                }

                var apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID);

                if (!apiResponse.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("Error", apiResponse.Message, "OK");

                    return;
                }

                if (apiResponse.Data.Count() == 0)
                {
                    await Shell.Current.DisplayAlert("Error", apiResponse.Message, "OK");

                    return;
                }

                foreach (var item in apiResponse.Data)
                {
                    var templateViewModel = new NearByTemplateViewModel(this.captureService, item, this.LogUserPerfilTool);

                    await templateViewModel.InitializeAsync();

                    this.NearByTemplateViewModels.Add(templateViewModel);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                this.IsLoading = true;

                this.NearByTemplateViewModels.Clear();

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

        public async void InitializeSettingsAsync()
        {
            await LoadSettings2Async();
        }

        protected override async void OpenFilterPopUp()
        {
            try
            {
                this._filterPopUpForSearch = new FilterPopUpForSearch(this.filterPopUpViewModel);

                await Shell.Current.CurrentPage.ShowPopupAsync(this._filterPopUpForSearch);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
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