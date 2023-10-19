using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class AppSettingService : BaseService<AppSetting>, IAppSettingService
    {
        public static string _apiSuffix = nameof(AppSettingService);

        public AppSettingService(HttpClient htppClient) : base(htppClient)
        {
        }

        public async Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByName(string name)
        {
            try
            {
                // Build the full API endpoint URL for the specific resource
                var apiUrl = $"{this.BaseApiUri}/{nameof(GetAppSettingByName)}/{name}";

                // Send the GET request to the API
                var response = await _httpClient.GetAsync(apiUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<AppSetting>>>(responseJson);

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<IEnumerable<AppSetting>>.BuildFail("Failed to get data.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, build a fail response with the error message
                var failResponse = ResponseFactory<IEnumerable<AppSetting>>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        public async Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByNames(IList<string> settinsName)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settinsName);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(GetAppSettingByNames)}", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<AppSetting>>>(responseJson);

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<IEnumerable<AppSetting>>.BuildFail("Failed to get data.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, build a fail response with the error message
                var failResponse = ResponseFactory<IEnumerable<AppSetting>>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }
    }
}