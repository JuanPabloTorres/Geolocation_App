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
    public partial class ManageLocationViewModel2 : BaseViewModel3<Advertisement, IAdvertisementService>
    {
        [ObservableProperty]
        private MapType seletedMapSetting;


        public ObservableCollection<Pin> Positions { get; set; } = new ObservableCollection<Pin>();

        public ObservableCollection<TemplateCardViewModel<GeolocationAd, IGeolocationAdService>> TemplateCardViewModel { get; set; } = new ObservableCollection<TemplateCardViewModel<GeolocationAd, IGeolocationAdService>>();

        public ObservableCollection<MapType> MapSettings { get; set; } = new ObservableCollection<MapType>(Enum.GetValues(typeof(MapType)).Cast<MapType>().ToList());

        private readonly IContainerManageLocation containerManageLocation;

        public ManageLocationViewModel2(IContainerManageLocation containerManageLocation) : base(containerManageLocation.Model, containerManageLocation.AdvertisementService, containerManageLocation.LogUserPerfilTool)
        {
            this.containerManageLocation = containerManageLocation;

            //var mapTypes = Enum.GetValues(typeof(MapType))
            //            .Cast<MapType>()
            //            .ToList();

            //this.MapSettings = new ObservableCollection<MapType>(mapTypes);

            this.ApplyQueryAttributesCompleted += ManageLocationViewModel_ApplyQueryAttributesCompleted;

            // Subscribe to the ItemDeletedEvent
            EventManager.Instance.Subscribe<GeolocationAd>(async (eventArgs) =>
            {
                // Handle the item deleted event here.
                await HandleItemDeletedEventAsync(eventArgs);
            });
        }

        private async Task HandleItemDeletedEventAsync(GeolocationAd eventArgs)
        {
            try
            {
                this.IsLoading = true;

                if (!eventArgs.IsObjectNull())
                {
                    var toRemoveViewModel = this.TemplateCardViewModel.FirstOrDefault(v => v.Model.ID == eventArgs.ID);

                    var toRemovePosition = this.Positions.FirstOrDefault(p => p.MarkerId.ToString() == eventArgs.ID.ToString());

                    if (!toRemoveViewModel.IsObjectNull() && !toRemovePosition.IsObjectNull())
                    {
                        this.TemplateCardViewModel.Remove(toRemoveViewModel);

                        this.Positions.Remove(toRemovePosition);
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
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
            try
            {
                this.IsLoading = true;

                this.Positions.Clear();

                this.TemplateCardViewModel.Clear();

                var _apiResponse = await this.service.Get(this.Model.ID);

                if (_apiResponse.IsSuccess)
                {
                    foreach (var geo in _apiResponse.Data.GeolocationAds)
                    {
                        AddPinToPositions(geo);

                        var _cardViewModel = new TemplateCardViewModel<GeolocationAd, IGeolocationAdService>(geo, this.containerManageLocation.GeolocationAdService);

                        this.TemplateCardViewModel.Add(_cardViewModel);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private async void ManageLocationViewModel_ApplyQueryAttributesCompleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                //this.Positions = new ObservableCollection<Pin>();

                //this.TemplateCardViewModel = new ObservableCollection<TemplateCardViewModel<GeolocationAd, IGeolocationAdService>>();

                foreach (var geo in this.Model.GeolocationAds)
                {
                    AddPinToPositions(geo);

                    var _cardViewModel = new TemplateCardViewModel<GeolocationAd, IGeolocationAdService>(geo, this.containerManageLocation.GeolocationAdService);

                    this.TemplateCardViewModel.Add(_cardViewModel);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private async void _cardViewModel_ItemDeleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                if (sender is TemplateCardViewModel<GeolocationAd, IGeolocationAdService> model)
                {
                    var _toRemove = this.TemplateCardViewModel.Where(v => v.Model.ID == model.Model.ID).FirstOrDefault();

                    var _positionToRemove = this.Positions.Where(v => v.Location.Latitude == model.Model.Latitude && v.Location.Longitude == model.Model.Longitude).FirstOrDefault();

                    this.Positions.Remove(_positionToRemove);

                    this.TemplateCardViewModel.Remove(_toRemove);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private void _pin_MarkerClicked(object sender, PinClickedEventArgs e)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task CreateAdToLocation()
        {
            try
            {
                this.IsLoading = true;

                var locationReponse = await GeolocationTool.GetLocation();

                if (!locationReponse.IsSuccess)
                {
                    await CommonsTool.DisplayAlert("Error", locationReponse.Message);

                    return;
                }

                GeolocationAd newLocation = GeolocationAdFactory.CreateGeolocationAd(this.Model.ID, locationReponse.Data);

                var _apiResponse = await this.containerManageLocation.GeolocationAdService.Add(newLocation);

                if (_apiResponse.IsSuccess)
                {
                    AddPinToPositions(_apiResponse.Data);

                    var _cardViewModel = new TemplateCardViewModel<GeolocationAd, IGeolocationAdService>(_apiResponse.Data, this.containerManageLocation.GeolocationAdService);

                    this.TemplateCardViewModel.Add(_cardViewModel);

                    await CommonsTool.DisplayAlert("Notification", _apiResponse.Message);
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }
    }
}
