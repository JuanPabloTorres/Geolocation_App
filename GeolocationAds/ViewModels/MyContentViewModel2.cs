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
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class MyContentViewModel2 : BaseViewModel3<ContentViewTemplateViewModel, IGeolocationAdService>
    {
        private readonly IContainerMyContentServices containerMyContentServices;

        [ObservableProperty]
        private AppSetting selectedAdType;

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; } = new ObservableCollection<AppSetting>();

        public MyContentViewModel2(IContainerMyContentServices myContentServices) : base(myContentServices.AdLocationTemplateViewModel, myContentServices.GeolocationAdService, myContentServices.LogUserPerfilTool)
        {
            this.containerMyContentServices = myContentServices;

            BaseTemplateViewModel.ItemDeleted += AdLocationTemplateViewModel_ItemDeleted;

            Task.Run(async () =>
            {
                await InitializeSettingsAsync();

                await InitializeAsync();
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

                var _sender = sender as FilterPopUpViewModel2;

                if (sender is FilterPopUpViewModel2 filterPopUpViewModel)
                {
                    this.CollectionModel.Clear();

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
                var _toRemoveAdContent = this.CollectionModel.Where(v => v.Advertisement.ID == model.Advertisement.ID).FirstOrDefault();

                this.CollectionModel.Remove(_toRemoveAdContent);
            }
        }

        protected override async Task LoadData(int? pageIndex = 1)
        {
            try
            {
                this.IsLoading = true;

                var userId = this.LogUserPerfilTool.GetUserId();

                var apiResponse = await containerMyContentServices.AdvertisementService
                                .GetAdvertisementsOfUser(userId, SelectedAdType.ID, pageIndex)
                                .ConfigureAwait(false);

                if (!apiResponse.IsSuccess)
                {
                    await CommonsTool.DisplayAlert("Error", apiResponse.Message);
                }

                var viewModels = apiResponse.Data.Select(ad => new ContentViewTemplateViewModel(this.containerMyContentServices.AdvertisementService, this.service, ad)).ToList();

                MainThread.BeginInvokeOnMainThread(() => CollectionModel.AddRange(viewModels));
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

        private async Task LoadSettingsAsync()
        {
            IList<string> settings = new List<string>() { SettingName.AdTypes.ToString() };

            try
            {
                this.IsLoading = true;

                if (this.AdTypesSettings.Any())
                {
                    this.AdTypesSettings.Clear();
                }

                var _apiResponse = await this.containerMyContentServices.AppSettingService.GetAppSettingByNames(settings);

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

                    filterPopUpViewModel = new FilterPopUpViewModel2(this.AdTypesSettings);

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

        public async Task InitializeSettingsAsync()
        {
            await LoadSettingsAsync();
        }

        public async Task InitializeAsync(int? pageIndex = 1)
        {
            await LoadData(pageIndex);
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