using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly HttpClient _httpClient;

        protected readonly string APIPrefix = "api/";

        protected readonly string ApiSuffix;

        protected Uri BaseApiUri;

        public BaseService(HttpClient httpClient, IConfiguration configuration)
        {
            var backendUrl = configuration.GetValue<string>("ApplicationSettings:GlobalLocalBackendUrl");

            string _httpResourceName = string.Empty;

#if DEBUG
            _httpResourceName = "BackendUrl";
#endif

#if IIS
                          _httpResourceName = "IISBackendUrl";
#endif

#if Release
                        _httpResourceName = "ProdBackendUrl";
#endif

            this._httpClient = httpClient;

            //_httpClient.Timeout = TimeSpan.FromMinutes(30); // Set the timeout here

            //var backendUrl = Application.Current.Resources[_httpResourceName] as string;

            this.BaseApiUri = new Uri($"{backendUrl}/{typeof(T).Name}", UriKind.RelativeOrAbsolute);
        }

        public void SetJwtToken(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        public virtual async Task<ResponseTool<T>> Add(T data)
        {
            return await HandleRequest<T>(async () =>
            {
                // Serializar el objeto a JSON
                var json = JsonConvert.SerializeObject(data);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST
                return await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(Add)}", content);
            });
        }

        public async Task<ResponseTool<T>> Get(int Id)
        {
            return await HandleRequest<T>(async () =>
            {
                var apiUrl = $"{this.BaseApiUri}/{nameof(Get)}/{Id}";

                return await _httpClient.GetAsync(apiUrl);
            });
        }

        public async Task<ResponseTool<IEnumerable<T>>> GetAll()
        {
            return await HandleRequest<IEnumerable<T>>(async () =>
            {
                var url = $"{this.BaseApiUri}/{nameof(GetAll)}";

                return await _httpClient.GetAsync(url);
            });
        }

        // This method must be in a class in a platform project, even if the HttpClient object is
        // constructed in a shared project.
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public async Task<ResponseTool<T>> Remove(int Id)
        {
            return await HandleRequest<T>(async () =>
            {
                var url = $"{this.BaseApiUri}/{nameof(Remove)}/{Id}";

                return await _httpClient.DeleteAsync(url);
            });
        }

        public virtual async Task<ResponseTool<T>> Update(T data, int currentId)
        {
            return await HandleRequest<T>(async () =>
            {
                var json = JsonConvert.SerializeObject(data);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var apiUrl = $"{this.BaseApiUri}/{nameof(Update)}/{currentId}";

                return await _httpClient.PutAsync(apiUrl, content);
            });
        }

        private bool IsTokenExpired(HttpResponseMessage response)
        {
            return response.StatusCode == System.Net.HttpStatusCode.Unauthorized;
        }

        public async Task<ResponseTool<TResponse>> HandleRequest<TResponse>(Func<Task<HttpResponseMessage>> httpCall)
        {
            try
            {
                if (!await IsConnectedToInternetAsync())
                {
                    return ResponseFactory<TResponse>.BuildFail("No estás conectado a Internet. Verifica tu conexión e inténtalo nuevamente.", default);
                }

                var response = await httpCall();

                if (IsTokenExpired(response))
                {
                    // Lógica para cerrar sesión automáticamente
                    await App.Current.Dispatcher.DispatchAsync(async () =>
                    {
                        await SecureLogoutAndRedirectToLogin(); // implementado abajo
                    });

                    return ResponseFactory<TResponse>.BuildFail("Token expirado. Cerrando sesión por seguridad.", default);
                }

                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseTool<TResponse>>(json);
            }
            catch (Exception ex)
            {
                return ResponseFactory<TResponse>.BuildFail($"Error: {ex.Message}", default);
            }
        }

        private async Task SecureLogoutAndRedirectToLogin()
        {
            // ⛔ Token expirado, notificar al ViewModel para cerrar sesión
            WeakReferenceMessenger.Default.Send(new SignOutMessage("SessionExpired"));
        }

        private async Task<bool> IsConnectedToInternetAsync()
        {
            var current = Connectivity.Current.NetworkAccess;

            return current == NetworkAccess.Internet;
        }
    }
}