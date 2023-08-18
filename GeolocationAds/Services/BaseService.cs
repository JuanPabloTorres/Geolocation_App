using Newtonsoft.Json;
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

        public BaseService()
        {
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

            this._httpClient = new HttpClient();

            var backendUrl = Application.Current.Resources[_httpResourceName] as string;

            this.BaseApiUri = new Uri($"{backendUrl}/{typeof(T).Name}", UriKind.RelativeOrAbsolute);
        }

        public BaseService(string apiSuffix)
        {
            this.ApiSuffix = apiSuffix;
        }

        public BaseService(HttpClient httpClient, string apiSuffix)
        {
            this._httpClient = new HttpClient();

            // Access the backend URL from the resource dictionary in App.xaml
            var backendUrl = Application.Current.Resources["BackendUrl"] as string;

            _httpClient = httpClient;

            ApiSuffix = apiSuffix;

            _httpClient.BaseAddress = new Uri($"{backendUrl}/{apiSuffix}");

            // Other HttpClient configurations can be done here if needed
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<ResponseTool<T>> Add(T data)
        {
            try
            {
                // Convert the data object to JSON
                var json = JsonConvert.SerializeObject(data);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Build the full API endpoint URL
                var apiUrl = APIPrefix + ApiSuffix;

                // Send the POST request to the API
                var response = await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(Add)}", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<T>>(responseJson);

                    // Build a success response with the data
                    //var successResponse = ResponseFactory<T>.BuildSusccess("Successfully added.", responseData);

                    //return successResponse;

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<T>.BuildFail("Failed to add.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, build a fail response with the error message

                var failResponse = ResponseFactory<T>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        public Task<ResponseTool<T>> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseTool<IEnumerable<T>>> GetAll()
        {
            try
            {
                // Build the full API endpoint URL for the "all" endpoint

                // Send an HTTP GET request to the "all" endpoint of your API
                HttpResponseMessage response = await this._httpClient.GetAsync($"{this.BaseApiUri}/{nameof(GetAll)}");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response content to your custom ResponseTool<IEnumerable<T>> type
                var result = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<T>>>(responseContent);

                return result;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the request
                // For simplicity, we'll just return an error ResponseTool with the exception message

                var failResponse = ResponseFactory<IEnumerable<T>>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        // This method must be in a class in a platform project, even if
        // the HttpClient object is constructed in a shared project.
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
            try
            {
                // Build the full API endpoint URL for the specific resource
                var apiUrl = $"{this.BaseApiUri}/{nameof(Remove)}/{Id}";

                // Send the DELETE request to the API
                var response = await _httpClient.DeleteAsync(apiUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<T>>(responseJson);

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<T>.BuildFail("Failed to remove.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, build a fail response with the error message
                var failResponse = ResponseFactory<T>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        public Task<ResponseTool<T>> Update(T data, int currentId)
        {
            throw new NotImplementedException();
        }
    }
}