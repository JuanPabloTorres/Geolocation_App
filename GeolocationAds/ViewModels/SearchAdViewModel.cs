using CommunityToolkit.Maui.Views;
using GeolocationAds.PopUps;
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
        private readonly IAppSettingService _appSettingService;

        private readonly ICaptureService _captureService;

        private readonly IAdvertisementService _ad;

        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        private ObservableCollection<string> _distanceSettings;
        public ObservableCollection<string> DistanceSettings => _distanceSettings ?? (_distanceSettings = new ObservableCollection<string>());

        private ObservableCollection<AppSetting> _adTypesSettings;
        public ObservableCollection<AppSetting> AdTypesSettings => _adTypesSettings ?? (_adTypesSettings = new ObservableCollection<AppSetting>());

        private ObservableCollection<NearByTemplateViewModel2> _nearByTemplateViewModels;
        public ObservableCollection<NearByTemplateViewModel2> NearByTemplateViewModels => _nearByTemplateViewModels ?? (_nearByTemplateViewModels = new ObservableCollection<NearByTemplateViewModel2>());

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

        public SearchAdViewModel(Advertisement advertisement, ICaptureService captureService, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUser) : base(advertisement, geolocationAdService, logUser)
        {
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

                    await InitializeAsync(PageIndex, isReset: true);
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

        //protected override async Task LoadData(object extraData, int? pageIndex = 1)
        //{
        //    ResponseTool<IEnumerable<Advertisement>> apiResponse;

        //    if (!(extraData is CurrentLocation currentLocation))
        //    {
        //        await DisplayError("Failed to retrieve location information. Please check your settings and try again.");

        //        return;
        //    }

        //    try
        //    {
        //        apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayError($"Error loading data: {ex.Message}");

        //        return;
        //    }

        //    if (!apiResponse.IsSuccess || !apiResponse.Data.Any())
        //    {
        //        await DisplayError(string.IsNullOrEmpty(apiResponse.Message) ? "No ads found." : apiResponse.Message);

        //        return;
        //    }

        //    //var tasks = apiResponse.Data.Select(async item =>
        //    //{
        //    //    var templateViewModel = new NearByTemplateViewModel(this._captureService, item, this.LogUserPerfilTool);

        //    //    await templateViewModel.InitializeAsync();

        //    //    return templateViewModel;
        //    //});

        //    //var tasks = apiResponse.Data.Select(async item =>
        //    //{
        //    //    var templateViewModel = new NearByTemplateViewModel2(this._captureService, item, this.LogUserPerfilTool);

        //    //    await templateViewModel.InitializeAsync();

        //    //    return templateViewModel;
        //    //});

        //    try
        //    {
        //        var viewModels = apiResponse.Data.Select(ad => new NearByTemplateViewModel2(this._captureService, ad, this.LogUserPerfilTool));

        //        this.NearByTemplateViewModels.AddRange(viewModels);

        //        // Parallel initialization
        //        await Task.WhenAll(this.NearByTemplateViewModels.Select(async item => await item.InitializeAsync()));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Consider how to handle partial failures and communicate them to the user
        //        await DisplayError($"An error occurred while loading ads: {ex.Message}");
        //    }

        //    //var viewModels = apiResponse.Data.Select(ad => new NearByTemplateViewModel2(this._captureService, ad,this.LogUserPerfilTool));

        //    //this.NearByTemplateViewModels.AddRange(viewModels);

        //    //// Parallel initialization
        //    //await Task.WhenAll(this.NearByTemplateViewModels.Select(async item => await item.InitializeAsync()));

        //    //try
        //    //{
        //    //    // Execute all initializations in parallel and then add them to the collection
        //    //    var results = await Task.WhenAll(tasks);

        //    //    foreach (var viewModel in results)
        //    //    {
        //    //        this.NearByTemplateViewModels.Add(viewModel);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    // Consider how to handle partial failures and communicate them to the user
        //    //    await DisplayError($"An error occurred while loading ads: {ex.Message}");
        //    //}
        //}

        protected override async Task LoadData(object extraData, int? pageIndex = 1)
        {
            if (!(extraData is CurrentLocation currentLocation))
            {
                await DisplayError("Failed to retrieve location information. Please check your settings and try again.");
                return;
            }

            ResponseTool<IEnumerable<Advertisement>> apiResponse;
            try
            {
                apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID);

                if (!apiResponse.IsSuccess || !apiResponse.Data.Any())
                {
                    string message = string.IsNullOrEmpty(apiResponse.Message) ? "No ads found." : apiResponse.Message;

                    await DisplayError(message);

                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayError($"Error loading data: {ex.Message}");
                return;
            }

            //List<NearByTemplateViewModel2> viewModels = apiResponse.Data.Select(ad => new NearByTemplateViewModel2(this._captureService, this,ad, this.LogUserPerfilTool)).ToList();

            //try
            //{
            //    // Parallel initialization of view models
            //    await Task.WhenAll(viewModels.Select(vm => vm.InitializeAsync()));

            //    // Safely add to collection if all initializations succeed
            //    this.NearByTemplateViewModels.AddRange(viewModels);
            //}
            //catch (Exception ex)
            //{
            //    // Handle partial failures here if necessary
            //    await DisplayError($"An error occurred while loading ads: {ex.Message}");
            //    // Decide how to handle already initialized viewModels if needed
            //}
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

        public async Task InitializeAsync(int? pageIndex = 1, bool? isReset = false)
        {
            this.IsLoading = true;

            if (isReset.HasValue && isReset.Value)
            {
                this.NearByTemplateViewModels.Clear();
            }

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

                await LoadData(_currentLocation, pageIndex);
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
    }
}