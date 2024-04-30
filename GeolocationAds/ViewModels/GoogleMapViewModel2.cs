using GeolocationAds.Services;
using Microsoft.Maui.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Tools;
using GeolocationAds.Tools;
using ToolsLibrary.Models;
using ToolsLibrary.Extensions;
using GeolocationAds.PopUps;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GeolocationAds.ViewModels
{
    public partial class GoogleMapViewModel2 : BaseViewModel3<Pin, IGeolocationAdService>
    {
        public IList<Pin> FoundLocations = new List<Pin>();

        private FilterPopUpViewModel filterPopUpViewModel;

        private const int DISTANCE_METER = 1000;

        [ObservableProperty]
        private AppSetting selectedAdType;

        public GoogleMapViewModel2(Pin model, IGeolocationAdService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
        }

        protected override async Task LoadData(int? pageIndex = 1)
        {
            var locationReponse = await GeolocationTool.GetLocation();

            if (locationReponse.IsSuccess)
            {
                var _currentLocation = new CurrentLocation(locationReponse.Data.Latitude, locationReponse.Data.Longitude);

                var _apiResponse = await this.service.FindAdsNearby(_currentLocation, DISTANCE_METER.ToString());

                this.CollectionModel.Clear();

                if (_apiResponse.IsSuccess)
                {
                    if (!_apiResponse.Data.IsObjectNull())
                    {
                        foreach (var adsGeoItem in _apiResponse.Data)
                        {
                            Location location = new Location()
                            {
                                Latitude = adsGeoItem.Latitude,
                                Longitude = adsGeoItem.Longitude,
                            };

                            this.CollectionModel.Add(new Microsoft.Maui.Controls.Maps.Pin()
                            {
                                Location = location,
                                Label = adsGeoItem.Advertisement.Title,
                                Address = adsGeoItem.Advertisement.Description,
                                Type = Microsoft.Maui.Controls.Maps.PinType.Generic,
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

        private async void FilterPopUpViewModel_FilterItem(object sender, EventArgs e)
        {
            try
            {
                await this._filterPopUp.CloseAsync();

                var _sender = sender as FilterPopUpViewModel;

                if (sender is FilterPopUpViewModel filterPopUpViewModel)
                {
                    this.SelectedAdType = filterPopUpViewModel.SelectedAdType;

                    Initialize();
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        protected override async Task OpenFilterPopUpAsync()
        {
            try
            {
                this._filterPopUp = new FilterPopUp(this.filterPopUpViewModel);

                await Shell.Current.CurrentPage.ShowPopupAsync(this._filterPopUp);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}