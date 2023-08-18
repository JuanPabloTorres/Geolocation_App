using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;

namespace GeolocationAds.ViewModels
{
    public class AdToLocationViewModel : BaseViewModel
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

        private IGeolocationAdService geolocationAdService { get; set; }

        public ICommand SearchAdCommand { get; set; }

        private Advertisement _selectedAdvertisement;

        public Advertisement SelectedAdvertisement
        {
            get => _selectedAdvertisement;
            set
            {
                if (_selectedAdvertisement != value)
                {
                    _selectedAdvertisement = value;

                    OnPropertyChanged();
                }
            }
        }

        public AdToLocationViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService)
        {
            this.advertisementService = advertisementService;

            this.geolocationAdService = geolocationAdService;

            this.Advertisements = new ObservableCollection<AdLocationTemplateViewModel>();

            this.SearchAdCommand = new Command(Initialize);

            WeakReferenceMessenger.Default.Register<DeleteItemMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var _toRemoveAdContent = Advertisements.Where(v => v.CurrentAdvertisement.ID == m.Value.ID).FirstOrDefault();

                    Advertisements.Remove(_toRemoveAdContent);
                });
            });
        }

        private async Task LoadData()
        {
            var _apiResponse = await this.advertisementService.GetAll();

            this.Advertisements.Clear();

            if (_apiResponse.IsSuccess)
            {
                IList<AdLocationTemplateViewModel> _adLocationTemplateViewModel = new List<AdLocationTemplateViewModel>();

                foreach (var item in _apiResponse.Data)
                {
                    var _item = new AdLocationTemplateViewModel(this.advertisementService, this.geolocationAdService)
                    {
                        CurrentAdvertisement = item,
                    };

                    _adLocationTemplateViewModel.Add(_item);
                }

                this.Advertisements.AddRange(_adLocationTemplateViewModel);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }
        }

        public async void Initialize()
        {
            IsLoading = true;

            await LoadData();

            IsLoading = false;
        }
    }
}