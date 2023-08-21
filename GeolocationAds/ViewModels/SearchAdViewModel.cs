using GeolocationAds.Services;
using GeolocationAds.Tools;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class SearchAdViewModel : BaseViewModel
    {
        private IGeolocationAdService geolocationAdService;

        //public ICommand SearchAdCommand { get; set; }

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

            //this.SearchAdCommand = new Command(Initialize);

            this.Advertisements = new ObservableCollection<Advertisement>();
        }

        private async Task LoadData(CurrentLocation currentLocation)
        {
            var _apiResponse = await this.geolocationAdService.FindAdNear(currentLocation);

            this.Advertisements.Clear();

            if (_apiResponse.IsSuccess)
            {
                if (!_apiResponse.Data.IsObjectNull())
                {
                    this.Advertisements.AddRange(_apiResponse.Data);
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

        [ICommand]
        public async void Initialize()
        {
            isLoading = true;

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

            isLoading = false;
        }
    }
}