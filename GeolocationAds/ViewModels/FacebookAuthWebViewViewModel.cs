using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace GeolocationAds.ViewModels
{
    public partial class FacebookAuthWebViewViewModel : ObservableObject
    {
        private readonly TaskCompletionSource<string> _taskCompletionSource;

        private const string CallbackUrl = "https://com.mycompany.myapp/";

        [ObservableProperty]
        private string facebookAuthUrl;

        public FacebookAuthWebViewViewModel()
        {
            //_taskCompletionSource = taskCompletionSource ?? throw new ArgumentNullException(nameof(taskCompletionSource));
            LoadFacebookLogin();
        }

        /// <summary>
        /// Carga la URL de autenticación de Facebook en el WebView.
        /// </summary>
        private void LoadFacebookLogin()
        {
            FacebookAuthUrl = $"https://www.facebook.com/v22.0/dialog/oauth" +
                              $"?client_id=2641020766093307" +
                              $"&redirect_uri={Uri.EscapeDataString(CallbackUrl)}" +
                              $"&response_type=token" +
                              $"&scope=email,public_profile";
        }

        /// <summary>
        /// Maneja la navegación en el WebView.
        /// </summary>
        [RelayCommand]
        public async Task OnNavigating(WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith(CallbackUrl))
            {
                e.Cancel = true; // 🔹 Cancelamos la navegación en WebView

                string accessToken = ExtractAccessToken(e.Url);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var userInfo = await GetFacebookUserInfoAsync(accessToken);
                    if (userInfo != null)
                    {
                        _taskCompletionSource.SetResult(userInfo.Id);
                    }
                    else
                    {
                        _taskCompletionSource.SetResult(null);
                    }
                }
                else
                {
                    _taskCompletionSource.SetResult(null);
                }
            }
        }

        /// <summary>
        /// Extrae el token de acceso de la URL.
        /// </summary>
        private string ExtractAccessToken(string url)
        {
            try
            {
                var uri = new Uri(url);
                if (!string.IsNullOrEmpty(uri.Fragment))
                {
                    var fragment = uri.Fragment.TrimStart('#'); // 🔹 Eliminamos `#`

                    var queryParams = HttpUtility.ParseQueryString(fragment);

                    return queryParams["access_token"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extrayendo el token: {ex.Message}");
            }
            return null;
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
        //private async Task<FacebookUserInfo> GetFacebookUserInfoAsync(string accessToken)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string requestUrl = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";
        //            var response = await client.GetStringAsync(requestUrl);
        //            return JsonSerializer.Deserialize<FacebookUserInfo>(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error obteniendo información del usuario: {ex.Message}");
        //        return null;
        //    }
        //}

        public async Task<FacebookUserInfo> GetFacebookUserInfoAsync(string accessToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // 🔹 Obtén tu App Secret desde la configuración
                    string appSecret = "583e310c4cb00083afe190305f0f4090";
                    
                    //string appSecret = _containerLoginServices.Configuration["FacebookSettings:FacebookAppSecret"];

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

                    return JsonSerializer.Deserialize<FacebookUserInfo>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo información del usuario: {ex.Message}");
                return null;
            }
        }


    }

    public class FacebookUserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
