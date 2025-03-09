using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using ToolsLibrary.Dto;

namespace GeolocationAds.ViewModels
{
    /// <summary>
    /// ViewModel que maneja la autenticación de Google usando WebView.
    /// </summary>
    public partial class GoogleAuthWebViewViewModel : ObservableObject
    {
        /// <summary>
        /// Evento que se activa cuando la autenticación se completa exitosamente.
        /// </summary>
        public Action<GoogleUserInfoDto> OnLoginCompleted { get; set; }

        /// <summary>
        /// URL que se cargará en el WebView para la autenticación con Google.
        /// </summary>
        [ObservableProperty]
        private string googleAuthUrl;

        private readonly IGoogleAuthService _googleAuthService;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor que recibe el servicio de autenticación de Google y la configuración de la aplicación.
        /// </summary>
        /// <param name="googleAuthService">Servicio que maneja la autenticación con Google.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        public GoogleAuthWebViewViewModel(IGoogleAuthService googleAuthService, IConfiguration configuration)
        {
            _googleAuthService = googleAuthService;

            _configuration = configuration;

            // Cargar la URL de autenticación con Google.
            LoadGoogleLogin();
        }

        /// <summary>
        /// Construye la URL de autenticación con Google y la asigna al WebView.
        /// </summary>
        private void LoadGoogleLogin()
        {
            // 🔹 Carga la URL de autenticación ya estructurada desde appsettings.json
            GoogleAuthUrl = _configuration.GetValue<string>("GoogleSettings:GoogleAuthUrl");

            if (string.IsNullOrEmpty(GoogleAuthUrl))
            {
                Console.WriteLine("⚠️ Error: GoogleAuthUrl no está configurado en appsettings.json.");
            }
        }

        /// <summary>
        /// Método para autenticar al usuario y obtener su información.
        /// </summary>
        /// <param name="authorizationCode">Código de autorización recibido de Google.</param>
        public async Task AuthenticateUserAsync(string authorizationCode)
        {
            try
            {
                // Obtiene el token de acceso utilizando el código de autorización.
                string accessToken = await _googleAuthService.AuthenticateAndRetrieveTokenAsync();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    // Obtiene la información del usuario autenticado.
                    var userInfo = await _googleAuthService.GetGoogleUserInfoAsync(accessToken);

                    if (userInfo != null)
                    {
                        // Dispara el evento con la información del usuario.
                        OnLoginCompleted?.Invoke(new GoogleUserInfoDto
                        {
                            Id = userInfo["id"]?.ToString(),
                            Name = userInfo["name"]?.ToString(),
                            Email = userInfo["email"]?.ToString(),
                            Picture = userInfo["picture"]?.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google Authentication failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un valor de configuración para Google desde el archivo de configuración.
        /// </summary>
        /// <param name="configName">Nombre del parámetro de configuración.</param>
        /// <returns>Valor del parámetro de configuración.</returns>
        public string GetGoogleConfiguration(string configName)
        {
            var value = _configuration.GetValue<string>($"GoogleSettings:{configName}");

            return !string.IsNullOrEmpty(value) ? value : string.Empty;
        }

    }


}
