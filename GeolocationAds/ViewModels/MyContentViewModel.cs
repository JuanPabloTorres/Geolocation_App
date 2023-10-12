using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class MyContentViewModel : BaseViewModel2<MangeContentTemplateViewModel, IGeolocationAdService>
    {
        private IList<string> settings = new List<string>() { SettingName.AdTypes.ToString() };

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

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        private FilterPopUpViewModel filterPopUpViewModel;

        public MyContentViewModel(MangeContentTemplateViewModel adLocationTemplateViewModel, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUserPerfilTool) : base(adLocationTemplateViewModel, geolocationAdService, logUserPerfilTool)
        {
            this.advertisementService = advertisementService;

            this.appSettingService = appSettingService;

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

            InitializeSettingsAsync();
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

                var _userId = this.LogUserPerfilTool.GetUserId();

                var _apiResponse = await this.advertisementService.GetAdvertisementsOfUser(_userId, this.SelectedAdType.ID);

                if (_apiResponse.IsSuccess)
                {
                    this.CollectionModel
                        .AddRange(_apiResponse.Data
                        .Select(s =>
                        new MangeContentTemplateViewModel(this.advertisementService, this.service, s))
                        .ToList());

                    foreach (var item in this.CollectionModel)
                    {
                        await item.InitializeAsync();
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
                        if (SettingName.AdTypes.ToString() == item.SettingName)
                        {
                            AdTypesSettings.Add(item);
                        }
                    }

                    SelectedAdType = AdTypesSettings.FirstOrDefault();

                    filterPopUpViewModel = new FilterPopUpViewModel(this.AdTypesSettings);

                    this.filterPopUpViewModel.OnFilterItem += FilterPopUpViewModel_FilterItem;
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

        public async void InitializeSettingsAsync()
        {
            await LoadSettings2Async();
        }

        public async void Initialize()
        {
            this.IsLoading = true;

            await LoadData();

            this.IsLoading = false;
        }

        protected override async void OpenFilterPopUp()
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