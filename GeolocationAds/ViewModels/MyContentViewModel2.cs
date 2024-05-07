using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Enums;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class MyContentViewModel2 : BaseViewModel3<ContentViewTemplateViewModel, IGeolocationAdService>
    {
        private IList<string> settings = new List<string>() { SettingName.AdTypes.ToString() };

        private readonly IAdvertisementService advertisementService;

        private readonly IAppSettingService appSettingService;

        private FilterPopUpViewModel filterPopUpViewModel;

        [ObservableProperty]
        private AppSetting selectedAdType;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();

        public MyContentViewModel2(ContentViewTemplateViewModel adLocationTemplateViewModel, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, IAppSettingService appSettingService, LogUserPerfilTool logUserPerfilTool) : base(adLocationTemplateViewModel, geolocationAdService, logUserPerfilTool)
        {
            this.advertisementService = advertisementService;

            this.appSettingService = appSettingService;

            ContentViewTemplateViewModel.ItemDeleted += AdLocationTemplateViewModel_ItemDeleted;

            WeakReferenceMessenger.Default.Register<UpdateMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await this.InitializeAsync();
                });
            });
        }

        [RelayCommand]
        public async Task Search()
        {
            PageIndex = 1;

            this.CollectionModel.Clear();

            await InitializeAsync();
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
            if (sender is ContentViewTemplateViewModel model)
            {
                var _toRemoveAdContent = this.CollectionModel.Where(v => v.CurrentAdvertisement.ID == model.CurrentAdvertisement.ID).FirstOrDefault();

                this.CollectionModel.Remove(_toRemoveAdContent);
            }
        }

        protected override async Task LoadData(int? pageIndex = 1)
        {
            try
            {
                this.IsLoading = true;

                var userId = this.LogUserPerfilTool.GetUserId();

                var apiResponse = await this.advertisementService.GetAdvertisementsOfUser(userId, this.SelectedAdType.ID, pageIndex);

                if (apiResponse.IsSuccess)
                {
                    // Convert the enumerable to a list to avoid multiple enumerations
                    var viewModels = apiResponse.Data.Select(ad => new ContentViewTemplateViewModel(this.advertisementService, this.service, ad)).ToList();

                    // Add the initialized viewModels to the collection
                    this.CollectionModel.AddRange(viewModels);
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

        [RelayCommand]
        public override async Task OpenFilterPopUp()
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