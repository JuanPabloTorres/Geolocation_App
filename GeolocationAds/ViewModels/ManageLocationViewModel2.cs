using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class ManageLocationViewModel2 : BaseViewModel<Advertisement, IAdvertisementService>
    {
        [ObservableProperty]
        private MapType seletedMapSetting = new();

        public ObservableCollection<Pin> Positions { get; set; } = new();

        public ObservableCollection<LocationCardViewModel<GeolocationAd, IGeolocationAdService>> LocationCardViewModels { get; set; } = new();

        public ObservableCollection<MapType> MapSettings { get; set; } = new(Enum.GetValues(typeof(MapType)).Cast<MapType>().ToObservableCollection());

        private readonly IContainerManageLocation containerManageLocation;

        public ManageLocationViewModel2(IContainerManageLocation containerManageLocation) : base(containerManageLocation.Model, containerManageLocation.AdvertisementService, containerManageLocation.LogUserPerfilTool)
        {
            this.containerManageLocation = containerManageLocation;

            this.ApplyQueryAttributesCompleted = async () => await ManageLocationViewModel_ApplyQueryAttributesCompleted();
        }

        private async void HandleItemDeletedEventAsync(GeolocationAd eventArgs)
        {
            await RunWithLoadingIndicator(async () =>
            {
                if (!eventArgs.IsObjectNull())
                {
                    var toRemoveViewModel = this.LocationCardViewModels.FirstOrDefault(v => v.Model.ID == eventArgs.ID);

                    var toRemovePosition = this.Positions.FirstOrDefault(p => p.MarkerId.ToString() == eventArgs.ID.ToString());

                    if (!toRemoveViewModel.IsObjectNull() && !toRemovePosition.IsObjectNull())
                    {
                        this.LocationCardViewModels.Remove(toRemoveViewModel);

                        this.Positions.Remove(toRemovePosition);
                    }
                }
            });
        }

        private void AddPinToPositions(GeolocationAd geo)
        {
            Location location = new Location()
            {
                Latitude = geo.Latitude,
                Longitude = geo.Longitude,
            };

            var _pin = new Microsoft.Maui.Controls.Maps.Pin()
            {
                Location = location,
                Label = this.Model.Title,
                Address = this.Model.Description,
                Type = Microsoft.Maui.Controls.Maps.PinType.Generic,
                MarkerId = geo.ID
            };

            this.Positions.Add(_pin);
        }

        [RelayCommand]
        protected override async Task LoadData(int? pageIndex = 1)
        {
            await RunWithLoadingIndicator(async () =>
            {
                this.Positions.Clear();

                this.LocationCardViewModels.Clear();

                var _apiResponse = await this.service.Get(this.Model.ID);

                if (_apiResponse.IsSuccess)
                {
                    foreach (var geo in _apiResponse.Data.GeolocationAds)
                    {
                        AddPinToPositions(geo);

                        var _cardViewModel = new LocationCardViewModel<GeolocationAd, IGeolocationAdService>(geo, this.containerManageLocation.GeolocationAdService, HandleItemDeletedEventAsync);

                        this.LocationCardViewModels.Add(_cardViewModel);
                    }
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                }
            });
        }

        private async Task ManageLocationViewModel_ApplyQueryAttributesCompleted()
        {
            await RunWithLoadingIndicator(async () =>
            {
                foreach (var geo in this.Model.GeolocationAds)
                {
                    AddPinToPositions(geo);

                    var _cardViewModel = new LocationCardViewModel<GeolocationAd, IGeolocationAdService>(geo, this.containerManageLocation.GeolocationAdService, HandleItemDeletedEventAsync);

                    this.LocationCardViewModels.Add(_cardViewModel);
                }
            });
        }

        private void _pin_MarkerClicked(object sender, PinClickedEventArgs e)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task CreateAdToLocation(Location? location)
        {
            await RunWithLoadingIndicator(async () =>
            {
                Location position;

                // Si no se pasan coordenadas, obtener la ubicación actual
                if (location.IsObjectNull())
                {
                    var locationResponse = await GeolocationTool.GetLocation();

                    if (!locationResponse.IsSuccess)
                    {
                        await CommonsTool.DisplayAlert("Error", locationResponse.Message);

                        return;
                    }

                    position = locationResponse.Data;
                }
                else
                {
                    // Usar las coordenadas proporcionadas
                    position = new Location(location.Latitude, location.Longitude);
                }

                var newLocation = GeolocationAdFactory.CreateGeolocationAd(this.Model.ID, position);

                var apiResponse = await this.containerManageLocation.GeolocationAdService.Add(newLocation);

                if (apiResponse.IsSuccess)
                {
                    AddPinToPositions(apiResponse.Data);

                    var cardViewModel = new LocationCardViewModel<GeolocationAd, IGeolocationAdService>(
                        apiResponse.Data,
                        this.containerManageLocation.GeolocationAdService,
                        HandleItemDeletedEventAsync
                    );

                    this.LocationCardViewModels.Add(cardViewModel);

                    await CommonsTool.DisplayAlert("Notification", apiResponse.Message);
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", apiResponse.Message);
                }
            });
        }
    }
}