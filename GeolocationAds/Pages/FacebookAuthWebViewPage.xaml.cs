using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Maui.Storage;
using GeolocationAds.ViewModels;
using Microsoft.Extensions.Configuration;
using ToolsLibrary.Tools;
using Firebase.Auth;

namespace GeolocationAds.Pages
{
    public partial class FacebookAuthWebViewPage : ContentPage
    {
        private FacebookAuthWebViewViewModel _viewModel;

        public FacebookAuthWebViewPage(FacebookAuthWebViewViewModel facebookAuthWebViewViewModel)
        {
            InitializeComponent();

            _viewModel = facebookAuthWebViewViewModel ?? throw new ArgumentNullException(nameof(facebookAuthWebViewViewModel));

            BindingContext = _viewModel;
        }

        public FacebookAuthWebViewPage()
        {
        }

        private async void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                var _callBackurl = _viewModel.GetFacebookConfiguration("FacebookRedirectUri");

                if (e.Url.StartsWith(_callBackurl))
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

                            // 🔹 Ejecutamos el callback para devolver la información
                            _viewModel.OnLoginCompleted?.Invoke(userInfo);
                            // Completar el TaskCompletionSource para que LoginViewModel2 reciba los datos
                            //_viewModel.                LoginCompletionSource?.TrySetResult(userInfo);
                        }
                        else
                        {
                            //_viewModel.LoginCompletionSource?.SetResult(null);

                            _viewModel.OnLoginCompleted?.Invoke(null);
                        }

                        await Navigation.PopAsync(); // 🔹 Cierra el WebView automáticamente
                    }
                    else
                    {
                        _viewModel.OnLoginCompleted?.Invoke(null);
                        //_taskCompletionSource.SetResult(null);

                        //_viewModel.LoginCompletionSource?.TrySetResult(null);

                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
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
    }

    //public class FacebookUserInfo
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //}
}