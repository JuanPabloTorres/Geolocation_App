using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using ToolsLibrary.Dto;

namespace GeolocationAds.ViewModels
{
    public partial class FacebookAuthWebViewViewModel : ObservableObject
    {
        public Action<FacebookUserInfoDto> OnLoginCompleted { get; set; }

        [ObservableProperty]
        private string facebookAuthUrl;

        protected IConfiguration Configuration { get; set; }

        public FacebookAuthWebViewViewModel(IConfiguration configuration)
        {
            this.Configuration = configuration;

            LoadFacebookLogin();
        }

        public string GetFacebookConfiguration(string configName)
        {
            var _configurationName = Configuration.GetValue<string>($"FacebookSettings:{configName}");

            return !String.IsNullOrEmpty(_configurationName) ? _configurationName : string.Empty;
        }

        /// <summary>
        /// Carga la URL de autenticación de Facebook en el WebView.
        /// </summary>
        private void LoadFacebookLogin()
        {
            string clientId = Configuration.GetValue<string>("FacebookSettings:FacebookAppId");

            string redirectUri = Configuration.GetValue<string>("FacebookSettings:FacebookRedirectUri");

            string authUrl = Configuration.GetValue<string>("FacebookSettings:FacebookAuthUrl");

            FacebookAuthUrl = $"{authUrl}" +
                $"?client_id={clientId}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                "&response_type=token" +
                $"&scope=email,public_profile";
        }

     

        // 🔹 Método para generar el appsecret_proof
        private string GenerateAppSecretProof(string accessToken, string appSecret)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(accessToken));

                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Obtiene la información del usuario desde Facebook Graph API.
        /// </summary>
        public async Task<FacebookUserInfoDto> GetFacebookUserInfoAsync(string accessToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // 🔹 Obtén tu App Secret desde la configuración
                    string appSecret = Configuration.GetValue<string>("FacebookSettings:FacebookAppSecret"); ;

                    // 🔹 Genera el appsecret_proof
                    string appSecretProof = GenerateAppSecretProof(accessToken, appSecret);

                    // 🔹 Construye la URL con el appsecret_proof
                    string requestUrl = $"https://graph.facebook.com/v22.0/me?" +
                                        $"fields=id,name,email" +
                                        $"&access_token={accessToken}" +
                                        $"&appsecret_proof={appSecretProof}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();

                        Console.WriteLine($"Facebook API Error: {errorMessage}");

                        throw new Exception($"Facebook API error: {errorMessage}");
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    return JsonSerializer.Deserialize<FacebookUserInfoDto>(jsonResponse, options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo información del usuario: {ex.Message}");
                return null;
            }
        }
    }

    //public class FacebookUserInfo
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //}
}