using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
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

            RemoveCommand = new Command<Advertisement>(RemoveContentYesOrNoAlert);

            this.onNavigate = new Command<int>(Navigate);
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

        public ICommand RemoveCommand { get; set; }

        public ICommand onNavigate { get; set; }

        private async Task CreateAdToLocation(Advertisement ad)
        {
            this.IsLoading = true;

            try
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
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            this.IsLoading = false;
        }

        private async Task RemoveContent(Advertisement ad)
        {
            this.IsLoading = true;

            var locationReponse = await GeolocationTool.GetLocation();

            if (locationReponse.IsSuccess)
            {
                var _apiResponse = await this.advertisementService.Remove(ad.ID);

                if (_apiResponse.IsSuccess)
                {
                    WeakReferenceMessenger.Default.Send(new DeleteItemMessage(ad));

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

            this.IsLoading = false;
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

        private async void RemoveContentYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $" Remove:{selectAd.Title}", "Yes", "No");

                if (response)
                {
                    await this.RemoveContent(selectAd);
                }
            }
        }

        private async void Navigate(int id)
        {
            var navigationParameter = new Dictionary<string, object> { { "ID", id } };

            await Shell.Current.GoToAsync(nameof(EditAdvertisment), navigationParameter);
        }
    }
}