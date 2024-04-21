using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class MyContentViewModel : BaseViewModel2<ContentViewTemplateViewModel, IGeolocationAdService>
    {
        //public static int PageIndex { get; set; } = 1;

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

        public MyContentViewModel(ContentViewTemplateViewModel adLocationTemplateViewModel, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUserPerfilTool) : base(adLocationTemplateViewModel, geolocationAdService, logUserPerfilTool)
        {
            this.advertisementService = advertisementService;

            this.appSettingService = appSettingService;

            this.AdTypesSettings = new ObservableCollection<AppSetting>();

            this.SearchCommand = new Command(async () =>
            {
                PageIndex = 1;

                this.CollectionModel.Clear();

                await InitializeAsync();
            });

            //this.OpenFilterPopUpCommand = new Command(OpenFilterPopUpAsync);

            OpenFilterPopUpCommand = new Command(async () => await OpenFilterPopUpAsync());

            MangeContentTemplateViewModel.ItemDeleted += AdLocationTemplateViewModel_ItemDeleted;

            WeakReferenceMessenger.Default.Register<UpdateMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await this.InitializeAsync();
                });
            });
        }

        private async void FilterPopUpViewModel_FilterItem(object sender, EventArgs e)
        {
            try
            {
                await this._filterPopUp.CloseAsync();

                this.CollectionModel.Clear();

                var _sender = sender as FilterPopUpViewModel;

                if (sender is FilterPopUpViewModel filterPopUpViewModel)
                {
                    this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                    await InitializeAsync();
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

        //protected override async Task LoadData()
        //{
        //    try
        //    {
        //        this.IsLoading = true;

        //        var _userId = this.LogUserPerfilTool.GetUserId();

        //        var _apiResponse = await this.advertisementService.GetAdvertisementsOfUser(_userId, this.SelectedAdType.ID, PageIndex);

        //        if (_apiResponse.IsSuccess)
        //        {
        //            this.CollectionModel
        //                .AddRange(_apiResponse.Data
        //                .Select(s =>
        //                new MangeContentTemplateViewModel(this.advertisementService, this.service, s))
        //                .ToList());

        //            foreach (var item in this.CollectionModel)
        //            {
        //                await item.InitializeAsync();
        //            }
        //        }
        //        else
        //        {
        //            await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
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

        protected override async Task LoadData(int? pageIndex = 1)
        {
            try
            {
                this.IsLoading = true;

                var userId = this.LogUserPerfilTool.GetUserId();

                var apiResponse = await this.advertisementService.GetAdvertisementsOfUser(userId, this.SelectedAdType.ID, pageIndex);

                if (apiResponse.IsSuccess)
                {
                    var viewModels = apiResponse.Data.Select(ad => new ContentViewTemplateViewModel(this.advertisementService, this.service, ad));

                    this.CollectionModel.AddRange(viewModels);

                    // Parallel initialization
                    await Task.WhenAll(this.CollectionModel.Select(async item => await item.InitializeAsync()));
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", apiResponse.Message, "OK");
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

        private async Task LoadSettings2Async()
        {
            try
            {
                this.IsLoading = true;

                this.AdTypesSettings.Clear();

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

        public async Task InitializeSettingsAsync()
        {
            await LoadSettings2Async();
        }

        public async Task InitializeAsync(int? pageIndex = 1)
        {
            await LoadData(pageIndex);
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