using GeolocationAds.Services;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public class SearchAdViewModel : BaseViewModel
    {
        private IGeolocationAdService geolocationAdService;

        public ICommand SearchAdCommand { get; set; }

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

        public SearchAdViewModel(IGeolocationAdService geolocationAdService)
        {
            this.geolocationAdService = geolocationAdService;

            this.SearchAdCommand = new Command(Initialize);
        }

        private async Task LoadData(CurrentLocation currentLocation)
        {
            var _apiResponse = await this.geolocationAdService.FindAdNear(currentLocation);

            if (_apiResponse.IsSuccess)
            {
                if (!_apiResponse.Data.IsObjectNull())
                {
                    this.Advertisements = new ObservableCollection<Advertisement>(_apiResponse.Data);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }
        }

        public async void Initialize()
        {
            IsLoading = true;

            var locationReponse = await GeolocationTool.GetLocation();

            if (locationReponse.IsSuccess)
            {
                var _currentLocation = new CurrentLocation(locationReponse.Data.Latitude, locationReponse.Data.Longitude);

                await LoadData(_currentLocation);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
            }

            IsLoading = false;
        }
    }
}