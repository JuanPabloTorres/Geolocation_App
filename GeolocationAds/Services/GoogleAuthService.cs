using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Maui.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models.Settings;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly HttpClient _httpClient;

        private readonly GoogleAuthSettings _googleAuthSettings;

        public GoogleAuthService(HttpClient httpClient, IOptions<GoogleAuthSettings> googleAuthOptions)
        {
            _httpClient = httpClient;

            _googleAuthSettings = googleAuthOptions.Value;
        }

        /// <summary>
        /// Autentica con Google y obtiene el token de acceso.
        /// </summary>
        public async Task<string> AuthenticateAndRetrieveTokenAsync()
        {
            try
            {
                var authUri = new Uri(_googleAuthSettings.GetAuthUrl());

                var callbackUri = new Uri(_googleAuthSettings.GoogleRedirectUri);

                var authResult = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions()
                {
                    Url = authUri,
                    CallbackUrl = callbackUri,
                });

                if (authResult.Properties.TryGetValue("code", out string authorizationCode))
                {
                    Console.WriteLine($"🔹 Código de autorización recibido: {authorizationCode}");

                    return await GetAccessTokenAsync(authorizationCode);
                }
                else
                {
                    throw new Exception("⚠️ No se obtuvo código de autorización");
                }
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
                var parameters = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", _googleAuthSettings.GoogleClientId),
                new KeyValuePair<string, string>("redirect_uri", _googleAuthSettings.GoogleRedirectUri),
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
                await CommonsTool.DisplayAlert("Error", $"Failed to exchange code for token: {ex.Message}");

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