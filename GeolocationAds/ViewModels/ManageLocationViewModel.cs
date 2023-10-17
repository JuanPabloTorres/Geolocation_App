using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class ManageLocationViewModel : BaseViewModel2<Advertisement, IAdvertisementService>
    {
        private readonly IGeolocationAdService geolocationAdService;

        public ICommand CreateCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        private ObservableCollection<Pin> _position;

        public ObservableCollection<Pin> Positions
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;

                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<TemplateCardViewModel<GeolocationAd, IGeolocationAdService>> _templateCardViewModels;

        public ObservableCollection<TemplateCardViewModel<GeolocationAd, IGeolocationAdService>> TemplateCardViewModel
        {
            get => _templateCardViewModels;
            set
            {
                if (_templateCardViewModels != value)
                {
                    _templateCardViewModels = value;

                    OnPropertyChanged();
                }
            }
        }

        private MapType _selectedMapSetting;

        public MapType SeletedMapSetting
        {
            get => _selectedMapSetting;
            set
            {
                if (_selectedMapSetting != value)
                {
                    _selectedMapSetting = value;

                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<MapType> MapSettings { get; set; }

        public ManageLocationViewModel(Advertisement model, IGeolocationAdService geolocationAdService, IAdvertisementService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.CreateCommand = new Command(async () => { await CreateAdToLocation(); });

            this.RefreshCommand = new Command(async () => { await LoadData(); });

            this.geolocationAdService = geolocationAdService;

            var mapTypes = Enum.GetValues(typeof(MapType))
                        .Cast<MapType>()
                        .ToList();

            this.MapSettings = new ObservableCollection<MapType>(mapTypes);



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

        private async void ManageLocationViewModel_ApplyQueryAttributesCompleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                this.Positions = new ObservableCollection<Pin>();

                this.TemplateCardViewModel = new ObservableCollection<TemplateCardViewModel<GeolocationAd, IGeolocationAdService>>();

                foreach (var geo in this.Model.GeolocationAds)
                {
                    AddPinToPositions(geo);

                    var _cardViewModel = new TemplateCardViewModel<GeolocationAd, IGeolocationAdService>(geo, this.geolocationAdService);

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

                var _apiResponse = await this.geolocationAdService.Add(newLocation);

                if (_apiResponse.IsSuccess)
                {
                    AddPinToPositions(_apiResponse.Data);

                    var _cardViewModel = new TemplateCardViewModel<GeolocationAd, IGeolocationAdService>(_apiResponse.Data, this.geolocationAdService);

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

        protected override async Task LoadData()
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

                        var _cardViewModel = new TemplateCardViewModel<GeolocationAd, IGeolocationAdService>(geo, this.geolocationAdService);

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
    }
}