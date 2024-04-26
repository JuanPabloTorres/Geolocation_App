using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFImageLoading;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using Org.BouncyCastle.Utilities;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.IO;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class SearchAdViewModel2 : BaseViewModel3<Advertisement, IGeolocationAdService>
    {
        private readonly IAppSettingService _appSettingService;

        private readonly ICaptureService _captureService;

        private readonly IAdvertisementService _advertisementService;

        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        [ObservableProperty]
        private string selectedDistance;

        [ObservableProperty]
        private AppSetting selectedAdType = new AppSetting();

        private FilterPopUpViewModel filterPopUpViewModel;

        public ObservableCollection<string> DistanceSettings { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();
        public ObservableCollection<NearByTemplateViewModel2> NearByTemplateViewModels { get; set; } = new ObservableCollection<NearByTemplateViewModel2>();

        private long starBlock = 0;

        private long endBlock = 1048576;

        private int PreviousContentId;

        public SearchAdViewModel2(Advertisement advertisement, ICaptureService captureService, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUser) : base(advertisement, geolocationAdService, logUser)
        {
            _appSettingService = appSettingService;

            _captureService = captureService;

            _advertisementService = advertisementService;

            SelectedAdType = new AppSetting();

            //NearByTemplateViewModel2.MediaElementPlaying += NearByTemplateViewModel2_MediaElementOpened;

            Task.Run(async () => { await InitializeSettingsAsync(); });
        }

        //private async void NearByTemplateViewModel2_MediaElementOpened(object sender, MediaStateChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (sender is NearByTemplateViewModel2 vm)
        //        {
        //            var fileSize = vm.Advertisement.Contents.FirstOrDefault().FileSize;

        //            var _newStartBlock = Convert.ToInt64(endBlock);

        //            var _newEndBlock = Convert.ToInt64( endBlock * 2);

        //            if (_newStartBlock >= fileSize)
        //            {
        //                // If start of new block is beyond file size, stop further downloads
        //                return;
        //            }

        //            // Adjust the end block to not exceed the file size
        //            _newEndBlock = Math.Min(_newEndBlock, fileSize - 1);

        //            var _blockRange = $"bytes={_newStartBlock}-{_newEndBlock}";

        //            var _contentId = vm.Advertisement.Contents.FirstOrDefault().ID;

        //            var _apiResponsVideoStream = await this._advertisementService.GetContentVideoAsync(_contentId, _blockRange);

        //            // Assume SaveByteArrayToPartialFile3 returns a boolean indicating success
        //            var success = await CommonsTool.SaveByteArrayToPartialFile3(_apiResponsVideoStream, vm.VideoFilePath, _newStartBlock, _newEndBlock);

        //            if (success != string.Empty)
        //            {
        //                await CommonsTool.DisplayAlert("Error", "Failed to save video segment.");

        //                return;
        //            }

        //            if (_newEndBlock >= fileSize - 1)
        //            {
        //                // All segments have been downloaded
        //                vm.MediaSource = vm.VideoFilePath; // Assuming MediaSource should point to the complete file path
        //                return;
        //            }

        //            // Update endBlock for the next segment
        //            endBlock = Convert.ToInt64(_newEndBlock + 1);

        //            //using (MemoryStream memoryStream = new MemoryStream())
        //            //{
        //            //    _apiResponsVideoStream.CopyTo(memoryStream);

        //            //    var _appendBytes = await CommonsTool.SaveByteArrayToPartialFile3(memoryStream.ToByteArray(), vm.VideoFilePath, _newStartBlock, _newEndBlock);

        //            //    vm.MediaSource =  _appendBytes;

        //            //}

        //            //var _appendBytes = await CommonsTool.SaveByteArrayToPartialFile3(_apiResponsVideoStream, vm.VideoFilePath, _newStartBlock, _apiResponsVideoStream.Length);

        //            //vm.MediaSource = _appendBytes;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await CommonsTool.DisplayAlert("Error", ex.Message);
        //    }
        //}

        private void SetBlockDefaultValue()
        {
            starBlock = 0;

            endBlock = 1048576;
        }

        private async void NearByTemplateViewModel2_MediaElementOpened(object sender, MediaStateChangedEventArgs e)
        {
            try
            {
                if (sender is NearByTemplateViewModel2 vm)
                {
                    var _contentId = vm.Advertisement.Contents.FirstOrDefault().ID;

                    var fileSize = vm.Advertisement.Contents.FirstOrDefault().FileSize; // Assuming size is known and stored

                    if (PreviousContentId == 0)
                    {
                        PreviousContentId = vm.Advertisement.ID;
                    }

                    if (PreviousContentId != vm.Advertisement.ID)
                    {
                        SetBlockDefaultValue();

                        PreviousContentId = vm.Advertisement.ID;
                    }

                    if (starBlock >= fileSize)
                    {
                        // If the start block is beyond the file size, no more downloads are needed

                        System.Diagnostics.Debug.WriteLine($"Stop Download  Reach File Size:{fileSize} Start Block:{starBlock}");

                        return;
                    }

                    // Calculate new end block, ensuring it does not exceed the file size
                    var _newEndBlock = Math.Min(starBlock + endBlock, fileSize);

                    var _blockRange = $"bytes={starBlock}-{_newEndBlock - 1}";  // Correct range specification

                    System.Diagnostics.Debug.WriteLine($"Start Block:{starBlock}");

                    System.Diagnostics.Debug.WriteLine($"End Block :{_newEndBlock}");

                    System.Diagnostics.Debug.WriteLine($"Range :{_blockRange}");

                    var _apiResponsVideoStream = await this._advertisementService.GetContentVideoAsync(_contentId, _blockRange);

                    var success = await CommonsTool.SaveByteArrayToPartialFile3(_apiResponsVideoStream, vm.VideoFilePath, starBlock, _newEndBlock - 1);

                    if (success == string.Empty)
                    {
                        await CommonsTool.DisplayAlert("Error", "Failed to save video segment.");

                        return;
                    }

                    // Update starBlock for the next segment
                    starBlock = _newEndBlock;

                    if (starBlock >= fileSize)
                    {
                        // All segments have been downloaded
                        //vm.MediaSource = null; // Assuming MediaSource should point to the complete file path
                       
                        //vm.MediaSource = vm.VideoFilePath; // Assuming MediaSource should point to the complete file path

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        [RelayCommand]
        public async Task Search()
        {
            await InitializeAsync(1, true);
        }

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
                    string message = string.IsNullOrEmpty(apiResponse.Message) ? "No ads found." : apiResponse.Message;

                    await CommonsTool.DisplayAlert("Error", message);

                    return;
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", $"Error loading data: {ex.Message}");

                return;
            }

            List<NearByTemplateViewModel2> viewModels = apiResponse.Data
                .Select(ad => new NearByTemplateViewModel2(this._captureService, ad, this.LogUserPerfilTool)).ToList();

            try
            {
                // Parallel initialization of view models
                await Task.WhenAll(viewModels.Select(vm => vm.InitializeAsync()));

                // Safely add to collection if all initializations succeed
                this.NearByTemplateViewModels.AddRange(viewModels);
            }
            catch (Exception ex)
            {
                // Handle partial failures here if necessary
                await CommonsTool.DisplayAlert("Error", $"An error occurred while loading ads: {ex.Message}");
                // Decide how to handle already initialized viewModels if needed
            }
        }

        public async Task InitializeSettingsAsync()
        {
            await LoadSettings2Async();
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

        [RelayCommand]
        public override async Task OpenFilterPopUp()
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