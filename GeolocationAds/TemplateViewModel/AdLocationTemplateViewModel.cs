using GeolocationAds.Services;
using GeolocationAds.Tools;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace ToolsLibrary.TemplateViewModel
{
    public class AdLocationTemplateViewModel : TemplateBaseViewModel
    {
        private Advertisement _currentAdvertisement;

        public AdLocationTemplateViewModel()
        {
            SetLocationCommand = new Command<Advertisement>(SetLocationYesOrNoAlert);
        }

        public AdLocationTemplateViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService) : base(advertisementService, geolocationAdService)
        {
            SetLocationCommand = new Command<Advertisement>(SetLocationYesOrNoAlert);
        }

        public Advertisement CurrentAdvertisement
        {
            get => _currentAdvertisement;
            set
            {
                if (_currentAdvertisement != value)
                {
                    _currentAdvertisement = value;

                    OnPropertyChanged();
                }
            }
        }

        public ICommand SetLocationCommand { get; set; }

        private async Task CreateAdToLocation(Advertisement ad)
        {
            var locationReponse = await GeolocationTool.GetLocation();

            if (locationReponse.IsSuccess)
            {
                GeolocationAd newAd = new GeolocationAd()
                {
                    AdvertisingId = ad.ID,
                    Advertisement = ad,
                    CreateDate = DateTime.Now,
                    Latitude = locationReponse.Data.Latitude,
                    Longitude = locationReponse.Data.Longitude
                };

                var _apiResponse = await this.geolocationAdService.Add(newAd);

                if (_apiResponse.IsSuccess)
                {
                    this.CurrentAdvertisement = _apiResponse.Data.Advertisement;

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
    }
}