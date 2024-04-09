using CommunityToolkit.Maui.Views;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class CaptureViewModel : BaseViewModel2<Capture, ICaptureService>
    {
        private IList<string> settings = new List<string>() { SettingName.AdTypes.ToString() };

        private readonly IAppSettingService appSettingService;
        public ObservableCollection<CaptureTemplateViewModel> CaptureTemplateViewModels { get; set; }

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        private FilterPopUpViewModel filterPopUpViewModel;

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

        public CaptureViewModel(Capture model, IAppSettingService appSettingService, ICaptureService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.appSettingService = appSettingService;

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.CaptureTemplateViewModels = new ObservableCollection<CaptureTemplateViewModel>();

            this.SearchCommand = new Command(Initialize);

            //this.OpenFilterPopUpCommand = new Command(OpenFilterPopUpAsync);

            this.OpenFilterPopUpCommand = new Command(async () => await OpenFilterPopUpAsync());

            CaptureTemplateViewModel.ItemDeleted += CaptureTemplateViewModel_ItemDeleted;

            InitializeSettingsAsync();
        }

        private async void CaptureTemplateViewModel_ItemDeleted(object sender, EventArgs e)
        {
            try
            {
                if (sender is CaptureTemplateViewModel model)
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
                    this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                    Initialize();
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
                    //foreach (var item in _apiResponse.Data)
                    //{
                    //    if (SettingName.AdTypes.ToString() == item.SettingName)
                    //    {
                    //        AdTypesSettings.Add(item);
                    //    }
                    //}

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

        public async void InitializeSettingsAsync()
        {
            await LoadSettings2Async();
        }

        protected override async Task LoadData()
        {
            try
            {
                this.IsLoading = true;

                this.CaptureTemplateViewModels.Clear();

                var _apiResponse = await this.service.GetMyCaptures(LogUserPerfilTool.GetUserId(), this.SelectedAdType.ID);

                if (_apiResponse.IsSuccess)
                {
                    this.CaptureTemplateViewModels
                       .AddRange(_apiResponse.Data
                       .Select(s =>
                       new CaptureTemplateViewModel(s, this.service))
                       .ToList());

                    foreach (var viewModel in this.CaptureTemplateViewModels)
                    {
                        await viewModel.InitializeAsync();
                    }
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

        public async void Initialize()
        {
            await LoadData();
        }

        protected override async Task OpenFilterPopUpAsync()
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