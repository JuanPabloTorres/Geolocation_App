using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class AdToLocationViewModel : BaseViewModel2<AdLocationTemplateViewModel, IGeolocationAdService>
    {
        private IAdvertisementService advertisementService { get; set; }

        public AdToLocationViewModel(AdLocationTemplateViewModel adLocationTemplateViewModel, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, LogUserPerfilTool logUserPerfilTool) : base(adLocationTemplateViewModel, geolocationAdService, logUserPerfilTool)
        {
            this.advertisementService = advertisementService;

            this.SearchCommand = new Command(Initialize);

            AdLocationTemplateViewModel.ItemDeleted += AdLocationTemplateViewModel_ItemDeleted;

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
        }

        private void AdLocationTemplateViewModel_ItemDeleted(object sender, EventArgs e)
        {
            if (sender is AdLocationTemplateViewModel model)
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
                        var _item = new AdLocationTemplateViewModel(this.advertisementService, this.service, item);

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

        public async void Initialize()
        {
            this.IsLoading = true;

            await LoadData();

            this.IsLoading = false;
        }
    }
}