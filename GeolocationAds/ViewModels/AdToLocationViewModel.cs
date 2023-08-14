using GeolocationAds.Services;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public class AdToLocationViewModel : BaseViewModel
    {
        private ObservableCollection<Advertisement> _advertisements;

        public ObservableCollection<Advertisement> Advertisements
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

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        private IAdvertisementService advertisementService { get; set; }

        private IGeolocationAdService geolocationAdService { get; set; }

        public ICommand SelectAdCommand { get; set; }

        private Advertisement _selectedAdvertisement;

        public Advertisement SelectedAdvertisement
        {
            get => _selectedAdvertisement;
            set
            {
                if (_selectedAdvertisement != value)
                {
                    _selectedAdvertisement = value;

                    SetLocationYesOrNoAlert(value);

                    OnPropertyChanged();
                }
            }
        }

        public AdToLocationViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService)
        {
            this.advertisementService = advertisementService;

            this.geolocationAdService = geolocationAdService;


        }

        private async void SetLocationYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $"Set Location To:{selectAd.Title}", "Yes", "No");

                if (response)
                {
                    await this.CreateAdToLocation(selectAd);
                }
            }
        }

        private async Task CreateAdToLocation(Advertisement ad)
        {
            var locationReponse = await GeolocationTool.GetLocation();

            if (locationReponse.IsSuccess)
            {
                GeolocationAd newAd = new GeolocationAd()
                {
                    AdvertisingId = ad.ID,
                    CreateDate = DateTime.Now,
                    Latitude = locationReponse.Data.Latitude,
                    Longitude = locationReponse.Data.Longitude
                };

                var _apiResponse = await this.geolocationAdService.Add(newAd);

                if (_apiResponse.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
            }
        }

        private async Task LoadData()
        {
            var _apiResponse = await this.advertisementService.GetAll();

            if (_apiResponse.IsSuccess)
            {
                this.Advertisements = new ObservableCollection<Advertisement>(_apiResponse.Data);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }
        }

        public async Task Initialize()
        {
            IsLoading = true;

            await LoadData();

            IsLoading = false;
        }
    }
}