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
        private readonly IAppSettingService _appSettingService;

        private readonly ICaptureService _captureService;

        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        private ObservableCollection<string> _distanceSettings;
        public ObservableCollection<string> DistanceSettings => _distanceSettings ?? (_distanceSettings = new ObservableCollection<string>());


        private ObservableCollection<AppSetting> _adTypesSettings;
        public ObservableCollection<AppSetting> AdTypesSettings => _adTypesSettings ?? (_adTypesSettings = new ObservableCollection<AppSetting>());


        private ObservableCollection<NearByTemplateViewModel> _nearByTemplateViewModels;
        public ObservableCollection<NearByTemplateViewModel> NearByTemplateViewModels => _nearByTemplateViewModels ?? (_nearByTemplateViewModels = new ObservableCollection<NearByTemplateViewModel>());


        private string _selectedDistance;
        public string SelectedDistance
        {
            get => _selectedDistance;
            set
            {
                if (_selectedDistance != value)
                {
                    _selectedDistance = value;
                    OnPropertyChanged(nameof(SelectedDistance));
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
                    OnPropertyChanged(nameof(SelectedAdType));
                }
            }
        }

        private FilterPopUpViewModel filterPopUpViewModel;

        public SearchAdViewModel(Advertisement advertisement, ICaptureService captureService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUser) : base(advertisement, geolocationAdService, logUser)
        {
            //this.appSettingService = appSettingService;

            //this.captureService = captureService;

            //this.DistanceSettings = new ObservableCollection<string>();

            //this.AdTypesSettings = new ObservableCollection<AppSetting>();

            //this.NearByTemplateViewModels = new ObservableCollection<NearByTemplateViewModel>();

            //this.SearchCommand = new Command(async () => await InitializeAsync());

            //this.OpenFilterPopUpCommand = new Command(OpenFilterPopUp);

            //this.SelectedAdType = new AppSetting();

            ////this.service.SetJwtToken(this.LogUserPerfilTool.JsonToken);

            //InitializeSettingsAsync();


            _appSettingService = appSettingService;

            _captureService = captureService;

            SearchCommand = new Command(async () => await InitializeAsync());

            OpenFilterPopUpCommand = new Command(async () => await OpenFilterPopUpAsync());

            SelectedAdType = new AppSetting();

            InitializeSettingsAsync();

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

                var _apiResponse = await this._appSettingService.GetAppSettingByNames(settings);

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

        //protected override async Task LoadData(object extraData)
        //{
        //    try
        //    {
        //        if (!(extraData is CurrentLocation currentLocation))
        //        {
        //            // Handle invalid 'extraData' if needed.
        //            await Shell.Current.DisplayAlert("Error", "Failed to retrieve location information. Please check your settings and try again.", "OK");

        //            return;
        //        }

        //        var apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID);

        //        if (!apiResponse.IsSuccess)
        //        {
        //            await Shell.Current.DisplayAlert("Error", apiResponse.Message, "OK");

        //            return;
        //        }

        //        if (apiResponse.Data.Count() == 0)
        //        {
        //            await Shell.Current.DisplayAlert("Error", apiResponse.Message, "OK");

        //            return;
        //        }

        //        foreach (var item in apiResponse.Data)
        //        {
        //            var templateViewModel = new NearByTemplateViewModel(this.captureService, item, this.LogUserPerfilTool);

        //            await templateViewModel.InitializeAsync();

        //            this.NearByTemplateViewModels.Add(templateViewModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await CommonsTool.DisplayAlert("Error", ex.Message);
        //    }
        //}

        protected override async Task LoadData(object extraData)
        {
            ResponseTool<IEnumerable<Advertisement>> apiResponse;

            if (!(extraData is CurrentLocation currentLocation))
            {
                await DisplayError("Failed to retrieve location information. Please check your settings and try again.");

                return;
            }

            try
            {
                apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID);
            }
            catch (Exception ex)
            {
                await DisplayError($"Error loading data: {ex.Message}");

                return;
            }

            if (!apiResponse.IsSuccess || !apiResponse.Data.Any())
            {
                await DisplayError(string.IsNullOrEmpty(apiResponse.Message) ? "No ads found." : apiResponse.Message);

                return;
            }

            var tasks = apiResponse.Data.Select(async item =>
            {
                var templateViewModel = new NearByTemplateViewModel(this._captureService, item, this.LogUserPerfilTool);

                await templateViewModel.InitializeAsync();

                return templateViewModel;
            });

            try
            {
                // Execute all initializations in parallel and then add them to the collection
                var results = await Task.WhenAll(tasks);

                foreach (var viewModel in results)
                {
                    this.NearByTemplateViewModels.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                // Consider how to handle partial failures and communicate them to the user
                await DisplayError($"An error occurred while loading ads: {ex.Message}");
            }
        }

        private static Task DisplayError(string message) => Shell.Current.DisplayAlert("Error", message, "OK");

        //public async Task InitializeAsync()
        //{
        //    try
        //    {
        //        this.IsLoading = true;

        //        this.NearByTemplateViewModels.Clear();

        //        var locationReponse = await GeolocationTool.GetLocation();

        //        if (locationReponse.IsSuccess)
        //        {
        //            var _currentLocation = new CurrentLocation(locationReponse.Data.Latitude, locationReponse.Data.Longitude);

        //            await LoadData(_currentLocation);
        //        }
        //        else
        //        {
        //            await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await CommonsTool.DisplayAlert("Error", ex.Message);
        //    }
        //    finally
        //    {
        //        this.IsLoading = false;
        //    }
        //}

        public async Task InitializeAsync()
        {
            this.IsLoading = true;

            this.NearByTemplateViewModels.Clear();

            try
            {
                var locationResponse = await GeolocationTool.GetLocation();

                if (!locationResponse.IsSuccess)
                {
                    // If location fetching fails, immediately display an error and exit the method.
                    await DisplayError(locationResponse.Message);
                    return; // Early exit to avoid further execution.
                }

                var _currentLocation = new CurrentLocation(locationResponse.Data.Latitude, locationResponse.Data.Longitude);

                await LoadData(_currentLocation);
            }
            catch (Exception ex)
            {
                await DisplayError($"Error initializing data: {ex.Message}");
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

        protected override async Task OpenFilterPopUpAsync()
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