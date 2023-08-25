using GeolocationAds.Tools;
using System.ComponentModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.Pages
{
    public partial class LocationPage : ContentPage, INotifyPropertyChanged
    {
        // Define la distancia del perímetro en metros
        private double perimeterDistanceInMeter = 10;

        // Propiedad para acceder y configurar la distancia del perímetro
        public double PerimeterDistanceInMeter
        {
            get => perimeterDistanceInMeter;
            set => perimeterDistanceInMeter = value;
        }

        // Lista de anuncios de geolocalización registrados
        public IList<GeolocationAd> GeolocationRegistered = new List<GeolocationAd>();

        // Lista de ubicaciones encontradas
        public IList<Location> FoundLocations = new List<Location>();

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LocationPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        // Evento para agregar contenido
        private async void AddContentButton_Clicked(object sender, EventArgs e)
        {
            // Muestra la carga en la interfaz de usuario
            Loading.IsVisible = true;

            try
            {
                // Genera un ID aleatorio
                Random randomId = new Random();

                // Solicita permiso para acceder a la ubicación del usuario
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    // Obtiene la ubicación del usuario
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        // Crea un nuevo objeto GeolocationAd
                        var _adGeolocation = new GeolocationAd()
                        {
                            ID = randomId.Next(1, 100),
                            Latitude = location.Latitude,
                            Longitude = location.Longitude,
                            Advertisement = new Advertisement()
                            {
                                Title = $"* Content {randomId.Next(1, 100)}",
                                Description = $"Ad Test {randomId.Next(1, 100)}"
                            }
                        };

                        // Agrega el anuncio de geolocalización a la lista
                        GeolocationRegistered.Add(_adGeolocation);

                        if (GeolocationRegistered.Contains(_adGeolocation))
                        {
                            // Crea un nuevo Label para mostrar el título del anuncio
                            Label _adContent = new Label()
                            {
                                Text = _adGeolocation.Advertisement.Title,
                            };

                            // Agrega el Label al contenedor de resultados
                            contentStacksResults.Children.Add(_adContent);
                        }
                    }
                    else
                    {
                        // No se pudo obtener la ubicación del usuario
                    }
                }
                else
                {
                    await DisplayAlert("Notification", "Not have permission for access location.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            // Oculta la carga en la interfaz de usuario
            Loading.IsVisible = false;
        }

        // Evento para limpiar el contenido
        private void ClearContentButton_Clicked(object sender, EventArgs e)
        {
            contentStacksResults.Children.Clear();
            findedStacksResults.Children.Clear();
            notfoundedStacksResults.Clear();
            GeolocationRegistered.Clear();
            FoundLocations.Clear();
            Loading.IsVisible = false;
        }

        // Evento para buscar contenido
        private async void FindContentButton_Clicked(object sender, EventArgs e)
        {
            Loading.IsVisible = true;
            findedStacksResults.Clear();
            notfoundedStacksResults.Clear();

            try
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);

                    var _currentLocation = await Geolocation.GetLocationAsync(request);

                    if (!_currentLocation.IsObjectNull())
                    {
                        FoundLocations.Add(_currentLocation);

                        foreach (var adsGeoItem in GeolocationRegistered)
                        {
                            Location _adGeolocation = new Location()
                            {
                                Latitude = adsGeoItem.Latitude,
                                Longitude = adsGeoItem.Longitude,
                            };

                            double meterDistance = GeolocationTool.VincentyFormula4(_currentLocation.Latitude, _currentLocation.Longitude, _adGeolocation.Latitude, _adGeolocation.Longitude);

                            //double meter = UnitConverterTool.KilometerToMeter(meterDistance);

                            if (meterDistance <= PerimeterDistanceInMeter)
                            {
                                Label _ad = new Label()
                                {
                                    Text = $"{adsGeoItem.Advertisement.Title} \n Location Request {_currentLocation.Latitude},{_currentLocation.Longitude}",
                                    BackgroundColor = Microsoft.Maui.Graphics.Colors.GreenYellow
                                };

                                findedStacksResults.Children.Add(_ad);
                            }
                            else
                            {
                                Label _notFoundLocation = new Label()
                                {
                                    Text = $" Location:{_currentLocation.Latitude},{_currentLocation.Longitude} \n Distance:{meterDistance}",
                                    BackgroundColor = Microsoft.Maui.Graphics.Colors.Red,
                                };

                                notfoundedStacksResults.Children.Add(_notFoundLocation);
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Location not found", "Sorry, we could not find your location.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Notification", "Not have permission for access location.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            Loading.IsVisible = false;
        }

        // Evento para abrir la página de Google Maps
        private async void googleMapClick(object sender, EventArgs e)
        {
            if (GeolocationRegistered.Count() > 0 || FoundLocations.Count() > 0)
            {
                //await this.Navigation.PushAsync(new GoogleMapPage(GeolocationRegistered, FoundLocations));
            }
            else
            {
                //await this.Navigation.PushAsync(new GoogleMapPage());
            }
        }

        // Evento para limpiar el contenido encontrado
        private void ClearContentFoundButton_Clicked(object sender, EventArgs e)
        {
            FoundLocations.Clear();
            findedStacksResults.Children.Clear();
        }
    }
}
