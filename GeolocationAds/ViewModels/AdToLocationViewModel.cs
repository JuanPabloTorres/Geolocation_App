using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class AdToLocationViewModel : BaseViewModel2<AdLocationTemplateViewModel, IGeolocationAdService>
    {
        private ObservableCollection<AdLocationTemplateViewModel> _advertisements;

        public ObservableCollection<AdLocationTemplateViewModel> Advertisements
        {
            get => _advertisements;
            set
            {
                if (_advertisements != value)
                {
                    _advertisements = value;

                    OnPropertyChanged();
                }
            }
        }

        private IAdvertisementService advertisementService { get; set; }

        public AdToLocationViewModel(AdLocationTemplateViewModel adLocationTemplateViewModel, IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, LogUserPerfilTool logUserPerfilTool) : base(adLocationTemplateViewModel, geolocationAdService, logUserPerfilTool)
        {
            this.advertisementService = advertisementService;

            this.Advertisements = new ObservableCollection<AdLocationTemplateViewModel>();

            this.SearchCommand = new Command(Initialize);

            WeakReferenceMessenger.Default.Register<DeleteItemMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var _toRemoveAdContent = Advertisements.Where(v => v.CurrentAdvertisement.ID == m.Value.ID).FirstOrDefault();

                    Advertisements.Remove(_toRemoveAdContent);
                });
            });
        }

        protected override async Task LoadData()
        {
            var _userId = this.LogUserPerfilTool.GetLogUserPropertyValue<int>(nameof(User.ID));

            var _apiResponse = await this.advertisementService.GetAdvertisementsOfUser(_userId);

            this.CollectionModel.Clear();

            if (_apiResponse.IsSuccess)
            {
                IList<AdLocationTemplateViewModel> _adLocationTemplateViewModel = new List<AdLocationTemplateViewModel>();

                foreach (var item in _apiResponse.Data)
                {
                    var _item = new AdLocationTemplateViewModel(this.advertisementService, this.service)
                    {
                        CurrentAdvertisement = item,
                    };

                    _adLocationTemplateViewModel.Add(_item);
                }

                this.CollectionModel.AddRange(_adLocationTemplateViewModel);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
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