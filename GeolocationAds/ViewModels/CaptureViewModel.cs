using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class CaptureViewModel : BaseViewModel<Capture, ICaptureService>
    {
        private IList<string> settings = new List<string>() { SettingName.AdTypes.ToString() };

        private readonly IContainerCapture containerCapture;

        [ObservableProperty]
        private AppSetting selectedAdType = new();

        private FilterPopUpViewModel filterPopUpViewModel;

        public ObservableCollection<CaptureTemplateViewModel2> CaptureTemplateViewModels { get; set; } = new();

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new();

        public CaptureViewModel(IContainerCapture containerCapture) : base(containerCapture.Model, containerCapture.CaptureService, containerCapture.LogUserPerfilTool)
        {
            this.containerCapture = containerCapture;

            Task.Run(async () =>
            {
                await InitializeSettingsAsync();

                await InitializeAsync();
            });

            RegisterForSignOutMessage();
        }

        public async Task InitializeSettingsAsync()
        {
            await LoadSettings2Async();
        }

        private async void CaptureTemplateViewModel_ItemDeleted(CaptureTemplateViewModel2 sender)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var toRemoveAdContent = CaptureTemplateViewModels
                    .FirstOrDefault(vm => vm.Capture.ID == sender.Capture.ID);

                if (!toRemoveAdContent.IsObjectNull())
                {
                    CaptureTemplateViewModels.Remove(toRemoveAdContent);

                    await CommonsTool.DisplayAlert("Notification", "Capture has been successfully removed.");
                }
            });
        }

        private async void FilterPopUpViewModel_FilterItem(FilterPopUpViewModel sender)
        {
            await RunWithLoadingIndicator(async () =>
            {
                await this._filterPopUp.CloseAsync();

                this.CaptureTemplateViewModels.Clear();

                this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                await InitializeAsync();
            });
        }

        private async Task LoadSettings2Async()
        {
            await RunWithLoadingIndicator(async () =>
            {
                var apiResponse = await containerCapture.AppSettingService.GetAppSettingByNames(settings);

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message); // Deja que RunWithLoadingIndicator maneje el error
                }

                var adTypes = apiResponse.Data.Where(item => item.SettingName == SettingName.AdTypes.ToString()).ToList();

                AdTypesSettings.AddRange(adTypes);

                SelectedAdType = AdTypesSettings.FirstOrDefault();

                filterPopUpViewModel = new FilterPopUpViewModel(AdTypesSettings);

                filterPopUpViewModel.OnFilterItem = FilterPopUpViewModel_FilterItem;
            });
        }

        public async Task InitializeAsync(int? pageIndex = 1)
        {
            await LoadData(pageIndex);
        }

        protected override async Task LoadData(int? pageIndex = 1)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var userId = LogUserPerfilTool.GetUserId();

                var apiResponse = await service.GetMyCaptures(userId, SelectedAdType.ID, pageIndex);

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message); // Deja que RunWithLoadingIndicator maneje el error
                }

                var captures = apiResponse.Data
                    .Select(s => new CaptureTemplateViewModel2(s, service, containerCapture.AdvertisementService, CaptureTemplateViewModel_ItemDeleted))
                    .ToList();

                CaptureTemplateViewModels.AddRange(captures);
            });
        }

        [RelayCommand]
        public async Task Search()
        {
            PageIndex = 1;

            this.CaptureTemplateViewModels.Clear();

            await InitializeAsync();
        }

        public override async Task OpenFilterPopUpForSearch()
        {
            try
            {
                this._filterPopUp = new FilterPopUp(this.filterPopUpViewModel);

                await Shell.Current.CurrentPage.ShowPopupAsync(this._filterPopUp);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        protected async override Task OnSignOutMessageReceivedAsync()
        {
            PageIndex = 1;

            this.CaptureTemplateViewModels.Clear();
        }
    }
}