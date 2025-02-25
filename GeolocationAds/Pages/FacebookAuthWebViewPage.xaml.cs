using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Maui.Storage;
using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages
{
    public partial class FacebookAuthWebViewPage : ContentPage
    {
        private readonly TaskCompletionSource<string> _taskCompletionSource;

        private const string CallbackUrl = "https://com.mycompany.myapp/";

        FacebookAuthWebViewViewModel _viewModel;

        public FacebookAuthWebViewPage()
        {
            InitializeComponent();

            BindingContext = new FacebookAuthWebViewViewModel();

            _viewModel=this.BindingContext as FacebookAuthWebViewViewModel; 
        }


        //public FacebookAuthWebViewPage(TaskCompletionSource<string> taskCompletionSource)
        //{
        //    InitializeComponent();

        //    //_taskCompletionSource = taskCompletionSource;

        //    _taskCompletionSource = taskCompletionSource ?? throw new ArgumentNullException(nameof(taskCompletionSource));

        //    LoadFacebookLogin();
        //}

       

        private void LoadFacebookLogin()
        {
            string facebookAuthUrl = $"https://www.facebook.com/v22.0/dialog/oauth" +
                                     $"?client_id=2641020766093307" +
                                     $"&redirect_uri={Uri.EscapeDataString(CallbackUrl)}" +
                                     $"&response_type=token" +
                                     $"&scope=email,public_profile";

            FacebookWebView.Source = new UrlWebViewSource() { Url=facebookAuthUrl};
        }

        private async void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith(CallbackUrl))
            {
                e.Cancel = true; // 🔹 Detenemos la navegación en el WebView

                string accessToken = ExtractAccessToken(e.Url);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine($"🔹 Token de acceso de Facebook: {accessToken}");

                    // 🔹 Obtener información del usuario desde la API de Facebook
                    var userInfo = await this._viewModel.GetFacebookUserInfoAsync(accessToken);

                    if (userInfo != null)
                    {
                        Console.WriteLine($"🔹 Facebook ID: {userInfo.Id}, Nombre: {userInfo.Name}");

                        // 🔹 Guardar Facebook ID en el backend (Aquí iría tu lógica para enviar la
                        // info al backend)
                        _taskCompletionSource.SetResult(userInfo.Id);
                    }
                    else
                    {
                        _taskCompletionSource.SetResult(null);
                    }

                    await Navigation.PopAsync(); // 🔹 Cierra el WebView automáticamente
                }
                else
                {
                    _taskCompletionSource.SetResult(null);
                    await Navigation.PopAsync();
                }
            }
        }

        private string ExtractAccessToken(string url)
        {
            try
            {
                var uri = new Uri(url);
                if (!string.IsNullOrEmpty(uri.Fragment))
                {
                    var fragment = uri.Fragment.TrimStart('#'); // 🔹 Eliminar `#`
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

        //private async Task<FacebookUserInfo> GetFacebookUserInfo(string accessToken)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string requestUrl = $"https://graph.facebook.com/v22.0/me?fields=id,name,email&access_token={accessToken}";
        //            HttpResponseMessage response = await client.GetAsync(requestUrl);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                string errorMessage = await response.Content.ReadAsStringAsync();

        //                Console.WriteLine($"Facebook API Error: {errorMessage}");

        //                throw new Exception($"Facebook API error: {errorMessage}");

        //                var jsonResponse = await response.Content.ReadAsStringAsync();

        //                return JsonSerializer.Deserialize<FacebookUserInfo>(jsonResponse);
        //            }
        //            else
        //            {
        //                return null;
        //            }

                  
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error obteniendo información del usuario: {ex.Message}");

        //        return null;
        //    }
        //}

    }

    public class FacebookUserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}