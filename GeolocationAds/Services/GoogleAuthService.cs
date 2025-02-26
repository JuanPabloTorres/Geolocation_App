using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IConfiguration _configuration;

        private readonly HttpClient _httpClient;

        public GoogleAuthService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;

            _httpClient = httpClient;
        }

        /// <summary>
        /// Autentica con Google y obtiene el token de acceso.
        /// </summary>
        public async Task<string> AuthenticateAndRetrieveTokenAsync()
        {
            try
            {
                string clientId = _configuration.GetValue<string>("GoogleSettings:GoogleClientId");

                string redirectUri = _configuration.GetValue<string>("GoogleSettings:GoogleRedirectUri");

                string authUrl = _configuration.GetValue<string>("GoogleSettings:GoogleAuthUrl");

                var authUri = new Uri(authUrl);

                var callbackUri = new Uri(redirectUri);

                var response = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions()
                {
                    Url = authUri,
                    CallbackUrl = callbackUri,
                    PrefersEphemeralWebBrowserSession = false,
                    

                });

                var authorizationCode = response.Properties["code"];

                return await GetAccessTokenAsync(authorizationCode);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", $"Google Authentication failed: {ex.Message}");

                return null;
            }
        }

        /// <summary>
        /// Intercambia el código de autorización por un token de acceso.
        /// </summary>
        private async Task<string> GetAccessTokenAsync(string authorizationCode)
        {
            try
            {
                string clientId = _configuration.GetValue<string>("GoogleSettings:GoogleClientId");
              
                string redirectUri = _configuration.GetValue<string>("GoogleSettings:GoogleRedirectUri");

                var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("code", authorizationCode),
                });

                var tokenResponse = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", parameters);

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve access token: {tokenResponse.ReasonPhrase}");
                }

                var jsonResponse = JObject.Parse(await tokenResponse.Content.ReadAsStringAsync());

                return jsonResponse["access_token"]?.ToString();
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error",$"Failed to exchange code for token: {ex.Message}");

                return null;
            }
        }

        /// <summary>
        /// Obtiene información del usuario desde Google con el token de acceso.
        /// </summary>
        public async Task<JObject> GetGoogleUserInfoAsync(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetStringAsync("https://www.googleapis.com/oauth2/v2/userinfo");

                return JObject.Parse(response);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", $"Failed to retrieve user info: {ex.Message}");

                return null;
            }
        }
    }
}