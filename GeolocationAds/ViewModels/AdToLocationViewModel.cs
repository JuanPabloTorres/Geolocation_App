using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Enums;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class AdToLocationViewModel : BaseViewModel2<MangeContentTemplateViewModel, IGeolocationAdService>
    {
        private IList<string> settings = new List<string>() { SettingName.MeterDistance.ToString(), SettingName.AdTypes.ToString() };

        private readonly IAdvertisementService advertisementService;

        private readonly IAppSettingService appSettingService;

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

        public ObservableCollection<string> DistanceSettings { get; set; }

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        private FilterPopUp filterPopUp;

        public ICommand OpenFilterPopUpCommand { get; set; }

        public AdToLocationViewModel(MangeContentTemplateViewModel adLocationTemplateViewModel, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUserPerfilTool) : base(adLocationTemplateViewModel, geolocationAdService, logUserPerfilTool)
        {
            this.advertisementService = advertisementService;

            this.appSettingService = appSettingService;

            this.DistanceSettings = new ObservableCollection<string>();

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.SearchCommand = new Command(Initialize);

            this.OpenFilterPopUpCommand = new Command(OpenFilterPopUp);

            MangeContentTemplateViewModel.ItemDeleted += AdLocationTemplateViewModel_ItemDeleted;

            WeakReferenceMessenger.Default.Register<LogOffMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.CollectionModel.Clear();
                });
            });

            WeakReferenceMessenger.Default.Register<UpdateMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.Initialize();
                });
            });

            InitializeSettings();
        }

        private void AdLocationTemplateViewModel_ItemDeleted(object sender, EventArgs e)
        {
            if (sender is MangeContentTemplateViewModel model)
            {
                var _toRemoveAdContent = this.CollectionModel.Where(v => v.CurrentAdvertisement.ID == model.CurrentAdvertisement.ID).FirstOrDefault();

                this.CollectionModel.Remove(_toRemoveAdContent);
            }
        }

        protected override async Task LoadData()
        {
            try
            {
                this.CollectionModel.Clear();

                var _userId = this.LogUserPerfilTool.GetLogUserPropertyValue<int>(nameof(User.ID));

                var _apiResponse = await this.advertisementService.GetAdvertisementsOfUser(_userId);

                if (_apiResponse.IsSuccess)
                {
                    foreach (var item in _apiResponse.Data)
                    {
                        var _item = new MangeContentTemplateViewModel(this.advertisementService, this.service, item);

                        this.CollectionModel.Add(_item);
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

        public async void Initialize()
        {
            this.IsLoading = true;

            await LoadData();

            this.IsLoading = false;
        }

        public async void InitializeSettings()
        {
            await LoadSettings2();
        }

        private async void OpenFilterPopUp()
        {
            try
            {
                var _viewModel = new FilterPopUpViewModel(this.appSettingService);

                this.filterPopUp = new FilterPopUp(_viewModel);

                await Shell.Current.CurrentPage.ShowPopupAsync(this.filterPopUp);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        //public void Dispose()
        //{
        //    this.CollectionModel.Clear();

        //    WeakReferenceMessenger.Default.Unregister<LogOffMessage>(this);

        //    WeakReferenceMessenger.Default.Unregister<UpdateMessage<Advertisement>>(this);

        //    MangeContentTemplateViewModel.ItemDeleted -= AdLocationTemplateViewModel_ItemDeleted;

        //    GC.SuppressFinalize(this);
        //}
    }
}