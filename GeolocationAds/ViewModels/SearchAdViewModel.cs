using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using GeolocationAds.Tools;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class SearchAdViewModel : BaseViewModel2<Advertisement, IGeolocationAdService>
    {
        public SearchAdViewModel(Advertisement advertisement, IGeolocationAdService geolocationAdService) : base(advertisement, geolocationAdService)
        {
            this.SearchCommand = new Command(Initialize);

            WeakReferenceMessenger.Default.Register<LogOffMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.CollectionModel.Clear();
                });
            });
        }

        protected override async Task LoadData(object extraData)
        {
            var currentLocation = extraData as CurrentLocation;

            var _apiResponse = await this.service.FindAdNear(currentLocation);

            this.CollectionModel.Clear();

            if (_apiResponse.IsSuccess)
            {
                if (!_apiResponse.Data.IsObjectNull())
                {
                    this.CollectionModel.AddRange(_apiResponse.Data);
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
            this.IsLoading = true;

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

            this.IsLoading = false;
        }
    }
}