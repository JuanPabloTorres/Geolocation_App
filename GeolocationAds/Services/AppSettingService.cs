using Newtonsoft.Json;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class AppSettingService : BaseService<AppSetting>, IAppSettingService
    {
        public static string _apiSuffix = nameof(AppSettingService);

        public AppSettingService()
        { }

        public AppSettingService(string _apiSuffix) : base(_apiSuffix)
        {
        }

        public AppSettingService(HttpClient htppClient, string _apiSuffix) : base(htppClient, _apiSuffix)
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
    }
}