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

        public BaseService(HttpClient httpClient)
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

            this._httpClient = httpClient;

            var backendUrl = Application.Current.Resources[_httpResourceName] as string;

            this.BaseApiUri = new Uri($"{backendUrl}/{typeof(T).Name}", UriKind.RelativeOrAbsolute);
        }

        public void SetJwtToken(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
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

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<T>.BuildFail("Bad Request.", null);

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

        public async Task<ResponseTool<T>> Get(int Id)
        {
            try
            {
                // Build the full API endpoint URL for the specific resource
                var apiUrl = $"{this.BaseApiUri}/{nameof(Get)}/{Id}";

                // Send the GET request to the API
                var response = await _httpClient.GetAsync(apiUrl);

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
                    var failResponse = ResponseFactory<T>.BuildFail("Bad Request.", null);

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
                    var failResponse = ResponseFactory<T>.BuildFail("Bad Request.", null);

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

        public async Task<ResponseTool<T>> Update(T data, int currentId)
        {
            try
            {
                // Convert the data object to JSON
                var json = JsonConvert.SerializeObject(data);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Build the full API endpoint URL for the specific resource and ID
                var apiUrl = $"{this.BaseApiUri}/{nameof(Update)}/{currentId}";

                // Send the PUT request to the API
                var response = await _httpClient.PutAsync(apiUrl, content);

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
                    var failResponse = ResponseFactory<T>.BuildFail("Bad Request.", null);

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
    }
}