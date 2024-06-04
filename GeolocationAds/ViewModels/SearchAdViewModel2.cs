using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class SearchAdViewModel2 : BaseViewModel3<Advertisement, IGeolocationAdService>
    {
        [ObservableProperty]
        private int carouselViewPosition;

        [ObservableProperty]
        private bool isNextVisible;

        [ObservableProperty]
        private bool isBackVisible;

        [ObservableProperty]
        private bool isNextEnabled;

        [ObservableProperty]
        private bool isBackEnabled;

        private readonly IAppSettingService _appSettingService;

        private readonly ICaptureService _captureService;

        private readonly IAdvertisementService _advertisementService;

        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        [ObservableProperty]
        private string selectedDistance;

        [ObservableProperty]
        private AppSetting selectedAdType = new AppSetting();

        public ObservableCollection<string> DistanceSettings { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();
        public ObservableCollection<NearByTemplateViewModel2> NearByTemplateViewModels { get; set; } = new ObservableCollection<NearByTemplateViewModel2>();

        public SearchAdViewModel2(Advertisement advertisement, ICaptureService captureService, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUser) : base(advertisement, geolocationAdService, logUser)
        {
            _appSettingService = appSettingService;

            _captureService = captureService;

            _advertisementService = advertisementService;

            SelectedAdType = new AppSetting();

            Task.Run(async () => { await InitializeDataLoadingSettingsAsync(); });
        }

        [RelayCommand]
        public async Task Search()
        {
            await InitializeDataLoadingAsync(1, true);
        }

        public async Task InitializeDataLoadingAsync(int? pageIndex = 1, bool? isReset = false)
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
                    await CommonsTool.DisplayAlert("Error", locationResponse.Message);
                    return; // Early exit to avoid further execution.
                }

                var _currentLocation = new CurrentLocation(locationResponse.Data.Latitude, locationResponse.Data.Longitude);

                await LoadData(_currentLocation, pageIndex);
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

        protected override async Task LoadData(object extraData, int? pageIndex = 1)
        {
            if (!(extraData is CurrentLocation currentLocation))
            {
                await CommonsTool.DisplayAlert("Error", "Failed to retrieve location information. Please check your settings and try again.");
                return;
            }

            ResponseTool<IEnumerable<Advertisement>> apiResponse;

            try
            {
                apiResponse = await this.service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID, pageIndex);

                if (!apiResponse.IsSuccess || !apiResponse.Data.Any())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", $"Error loading data: {ex.Message}");

                return;
            }

            List<NearByTemplateViewModel2> viewModels = apiResponse.Data.Distinct().Select(ad => new NearByTemplateViewModel2(this._captureService, this._advertisementService, ad, this.LogUserPerfilTool)).ToList();

            try
            {
                this.NearByTemplateViewModels.AddRange(viewModels);
            }
            catch (Exception ex)
            {
                // Handle partial failures here if necessary
                await CommonsTool.DisplayAlert("Error", $"An error occurred while loading ads: {ex.Message}");
                // Decide how to handle already initialized viewModels if needed
            }
            finally
            {
                this.IsNextVisible = true;

                this.IsBackVisible = true;

                this.IsBackEnabled = false;

                this.IsNextEnabled = true;
            }
        }

        public async Task InitializeDataLoadingSettingsAsync()
        {
            await LoadSettings2Async();
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

                    await InitializeDataLoadingAsync(PageIndex, isReset: true);
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
    }
}