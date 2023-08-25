using GeolocationAds.Services;
using GeolocationAds.Tools;
using Microsoft.Maui.Controls.Maps;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class GoogleMapViewModel : BaseViewModel2<Pin, IGeolocationAdService>
    {
        public IList<Pin> FoundLocations = new List<Pin>();

        public GoogleMapViewModel(Pin model, IGeolocationAdService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            //Task.Run(async () => { await LoadData(); });

        }

        protected override async Task LoadData()
        {
            var locationReponse = await GeolocationTool.GetLocation();

            if (locationReponse.IsSuccess)
            {
                var _currentLocation = new CurrentLocation(locationReponse.Data.Latitude, locationReponse.Data.Longitude);

                var _apiResponse = await this.service.FindAdNear(_currentLocation);

                this.CollectionModel.Clear();

                if (_apiResponse.IsSuccess)
                {
                    if (!_apiResponse.Data.IsObjectNull())
                    {
                        foreach (var adsGeoItem in _apiResponse.Data)
                        {
                            Location location = new Location()
                            {
                                Latitude = adsGeoItem.GeolocationAd.Latitude,
                                Longitude = adsGeoItem.GeolocationAd.Longitude,
                            };

                            this.CollectionModel.Add(new Microsoft.Maui.Controls.Maps.Pin()
                            {
                                Location = location,
                                Label = adsGeoItem.Title,
                                Address = $"LA:{location.Latitude} Lo:{location.Longitude}",
                                Type = Microsoft.Maui.Controls.Maps.PinType.SavedPin,
                            });
                        }
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
            else
            {
                await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
            }
        }

        public async Task Initialize()
        {
            await this.LoadData();
        }

        public IEnumerable<Pin> GetContentPins()
        {
            return this.CollectionModel.ToList();
        }
    }
}