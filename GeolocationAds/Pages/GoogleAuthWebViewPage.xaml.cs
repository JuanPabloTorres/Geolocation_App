using Microsoft.Maui.Controls;
using System;
using System.Web;
using GeolocationAds.ViewModels;
using Microsoft.Extensions.Configuration;
using ToolsLibrary.Tools;
using System.Threading.Tasks;

namespace GeolocationAds.Pages
{
    public partial class GoogleAuthWebViewPage : ContentPage
    {
        private readonly GoogleAuthWebViewViewModel _viewModel;

        /// <summary>
        /// Constructor que recibe el ViewModel y establece el contexto de enlace.
        /// </summary>
        /// <param name="googleAuthWebViewViewModel">ViewModel que maneja la autenticación con Google.</param>
        public GoogleAuthWebViewPage(GoogleAuthWebViewViewModel googleAuthWebViewViewModel)
        {
            InitializeComponent();

            _viewModel = googleAuthWebViewViewModel ?? throw new ArgumentNullException(nameof(googleAuthWebViewViewModel));

            BindingContext = _viewModel;
        }

        public GoogleAuthWebViewPage()
        {
        }

        /// <summary>
        /// Evento que se activa cuando el WebView navega a una nueva URL.
        /// Se usa para capturar el código de autorización de Google.
        /// </summary>
        private async void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                // URL de redirección configurada en Google Cloud
                var callbackUrl = _viewModel.GetGoogleConfiguration("GoogleRedirectUri");

                if (e.Url.StartsWith(callbackUrl))
                {
                    e.Cancel = true; // 🔹 Detener la navegación dentro del WebView

                    string authorizationCode = ExtractAuthorizationCode(e.Url);

                    if (!string.IsNullOrEmpty(authorizationCode))
                    {
                        Console.WriteLine($"🔹 Código de autorización de Google: {authorizationCode}");

                        // 🔹 Obtener token de acceso e información del usuario
                        await _viewModel.AuthenticateUserAsync(authorizationCode);

                        // 🔹 Cierra la página WebView automáticamente
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        _viewModel.OnLoginCompleted?.Invoke(null);

                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", $"Google Sign-in failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Extrae el código de autorización de la URL de redirección de Google.
        /// </summary>
        /// <param name="url">URL redirigida después del inicio de sesión en Google.</param>
        /// <returns>Código de autorización si existe, de lo contrario, null.</returns>
        private string ExtractAuthorizationCode(string url)
        {
            try
            {
                var uri = new Uri(url);

                var queryParams = HttpUtility.ParseQueryString(uri.Query);

                return queryParams["code"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extrayendo el código de autorización: {ex.Message}");
            }
            return null;
        }
    }
}
