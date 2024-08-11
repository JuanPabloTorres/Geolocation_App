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
    public partial class CaptureViewModel2 : BaseViewModel3<Capture, ICaptureService>
    {
        private IList<string> settings = new List<string>() { SettingName.AdTypes.ToString() };

        //private readonly IAppSettingService appSettingService;

        //private readonly IAdvertisementService advertisementService;

        private readonly IContainerCapture containerCapture;

        [ObservableProperty]
        private AppSetting selectedAdType = new AppSetting();

        private FilterPopUpViewModel filterPopUpViewModel;

        public ObservableCollection<CaptureTemplateViewModel2> CaptureTemplateViewModels { get; set; } = new ObservableCollection<CaptureTemplateViewModel2>();

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();

        public CaptureViewModel2(IContainerCapture containerCapture) : base(containerCapture.Model, containerCapture.CaptureService, containerCapture.LogUserPerfilTool)
        {
            this.containerCapture = containerCapture;

            Task.Run(async () =>
            {
                await InitializeSettingsAsync();

                await InitializeAsync();
            });
            // Subscribe to the ItemDeletedEvent
            EventManager2.Instance.Subscribe<CaptureTemplateViewModel2>(async (eventArgs) => { await HandleItemDeletedEventAsync(eventArgs); }, this);
        }

        private async Task HandleItemDeletedEventAsync(CaptureTemplateViewModel2 eventArgs)
        {
            try
            {
                var _toRemoveAdContent = this.CaptureTemplateViewModels.FirstOrDefault(vm => vm.Capture.ID == eventArgs.Capture.ID);

                if (!_toRemoveAdContent.IsObjectNull())
                {
                    this.CaptureTemplateViewModels.Remove(_toRemoveAdContent);

                    await CommonsTool.DisplayAlert("Notification", "Capture has been successfully removed.");
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public async Task InitializeSettingsAsync()
        {
            await LoadSettings2Async();
        }

        private async void CaptureTemplateViewModel_ItemDeleted(object sender, EventArgs e)
        {
            try
            {
                if (sender is CaptureTemplateViewModel2 model)
                {
                    var _toRemoveAdContent = this.CaptureTemplateViewModels.FirstOrDefault(vm => vm.Capture.ID == model.Capture.ID);

                    if (!_toRemoveAdContent.IsObjectNull())
                    {
                        this.CaptureTemplateViewModels.Remove(_toRemoveAdContent);

                        await CommonsTool.DisplayAlert("Notification", "Capture has been successfully removed.");
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private async void FilterPopUpViewModel_FilterItem(object sender, EventArgs e)
        {
            try
            {
                await this._filterPopUp.CloseAsync();

                var _sender = sender as FilterPopUpViewModel;

                if (sender is FilterPopUpViewModel filterPopUpViewModel)
                {
                    this.CaptureTemplateViewModels.Clear();

                    this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

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

                var _apiResponse = await this.containerCapture.AppSettingService.GetAppSettingByNames(settings);

                if (_apiResponse.IsSuccess)
                {
                    AdTypesSettings.AddRange(_apiResponse.Data.Where(item => item.SettingName == SettingName.AdTypes.ToString()).ToList());

                    SelectedAdType = AdTypesSettings.FirstOrDefault();

                    filterPopUpViewModel = new FilterPopUpViewModel(this.AdTypesSettings);

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

        public async Task InitializeAsync(int? pageIndex = 1)
        {
            CaptureTemplateViewModel2.CurrentPageContext = this.GetType().Name;

            await LoadData(pageIndex);
        }

        protected override async Task LoadData(int? pageIndex = 1)
        {
            try
            {
                this.IsLoading = true;

                var _apiResponse = await this.service.GetMyCaptures(LogUserPerfilTool.GetUserId(), this.SelectedAdType.ID, pageIndex);

                if (_apiResponse.IsSuccess)
                {
                    this.CaptureTemplateViewModels
                       .AddRange(
                        _apiResponse.Data.Select(s => new CaptureTemplateViewModel2(s, this.service, this.containerCapture.AdvertisementService))
                       .ToList());
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
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
        public async Task Search()
        {
            PageIndex = 1;

            if (this.CaptureTemplateViewModels.Any())
            {
                this.CaptureTemplateViewModels.Clear();
            }

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
    }
}