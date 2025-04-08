using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class SearchAdViewModel : BaseViewModel<Advertisement, IGeolocationAdService>
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

        public readonly IContainerSearchAdServices _containerSearchAdServices;

        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        [ObservableProperty]
        private string selectedDistance;

        [ObservableProperty]
        private AppSetting selectedAdType = new();

        public ObservableCollection<string> DistanceSettings { get; set; } = new();
        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new();
        public ObservableCollection<NearByTemplateViewModel2> NearByTemplateViewModels { get; set; } = new();

        public SearchAdViewModel(IContainerSearchAdServices containerSearchAdServices) : base(containerSearchAdServices.Advertisement, containerSearchAdServices.GeolocationAdService, containerSearchAdServices.LogUserPerfilTool)
        {
            _containerSearchAdServices = containerSearchAdServices;

            SelectedAdType = new AppSetting();

            Task.Run(async () => { await InitializeDataLoadingSettingsAsync(); });

            RegisterForSignOutMessage();
        }

        [RelayCommand]
        public async Task Search()
        {
            await InitializeDataLoadingAsync(1, true);
        }

        public async Task InitializeDataLoadingAsync(int? pageIndex = 1, bool? isReset = false)
        {
            await RunWithLoadingIndicator(async () =>
            {
                if (isReset == true)
                {
                    NearByTemplateViewModels.Clear();
                }

                var locationResponse = await GeolocationTool.GetLocation();

                if (!locationResponse.IsSuccess)
                    throw new Exception(locationResponse.Message); // Se lanza para que el RunWithLoading lo maneje.

                var currentLocation = new CurrentLocation(locationResponse.Data.Latitude, locationResponse.Data.Longitude);

                await LoadData(currentLocation, pageIndex);
            });
        }

        protected override async Task LoadData(object extraData, int? pageIndex = 1)
        {
            if (extraData is not CurrentLocation currentLocation)
            {
                throw new Exception("Failed to retrieve location information. Please check your settings and try again.");
            }

            var apiResponse = await service.FindAdNear2(currentLocation, SelectedDistance, SelectedAdType.ID, pageIndex);

            if (!apiResponse.IsSuccess)
                throw new Exception(apiResponse.Message);

            var viewModels = apiResponse.Data
                .Distinct()
                .Select(ad => new NearByTemplateViewModel2(_containerSearchAdServices.CaptureService, _containerSearchAdServices.AdvertisementService, ad, LogUserPerfilTool))
                .ToList();

            NearByTemplateViewModels.AddRange(viewModels);

            // Control de visibilidad y botones
            bool hasItems = NearByTemplateViewModels.Count > 0;

            IsNextVisible = hasItems;

            IsBackVisible = hasItems;

            IsBackEnabled = false; // Siempre inicia deshabilitado

            IsNextEnabled = hasItems;
        }

        public async Task InitializeDataLoadingSettingsAsync()
        {
            await LoadSettingsAsync();
        }

        private async void FilterPopUpViewModel_FilterItem()
        {
            try
            {
                await this._filterPopUpForSearch.CloseAsync();

                PageIndex = 1;

                this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                this.SelectedDistance = filterPopUpViewModel.SelectedDistance;

                await InitializeDataLoadingAsync(PageIndex, isReset: true);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private async Task LoadSettingsAsync()
        {
            await RunWithLoadingIndicator(async () =>
            {
                var _apiResponse = await _containerSearchAdServices.AppSettingService.GetAppSettingByNames(settings);

                if (!_apiResponse.IsSuccess)
                    throw new Exception(_apiResponse.Message);

                foreach (var item in _apiResponse.Data)
                {
                    switch (item.SettingName)
                    {
                        case var name when name == SettingName.MeterDistance.ToString():
                            DistanceSettings.Add(item.Value);
                            break;

                        case var name when name == SettingName.AdTypes.ToString():
                            AdTypesSettings.Add(item);
                            break;
                    }
                }

                SelectedAdType = AdTypesSettings.FirstOrDefault();

                SelectedDistance = DistanceSettings.FirstOrDefault();

                filterPopUpViewModel = new FilterPopUpViewModel2(AdTypesSettings, DistanceSettings);

                filterPopUpViewModel.OnFilterItem = FilterPopUpViewModel_FilterItem;
            });
        }

        protected async override Task OnSignOutMessageReceivedAsync()
        {
            PageIndex = 1;

            this.NearByTemplateViewModels.Clear();
        }
    }
}