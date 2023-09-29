using Newtonsoft.Json;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class CaptureService : BaseService<Capture>, ICaptureService
    {
        public static string _apiSuffix = nameof(CaptureService);

        public CaptureService()
        { }

        public CaptureService(string _apiSuffix) : base(_apiSuffix)
        {
        }

        public CaptureService(HttpClient htppClient, string _apiSuffix) : base(htppClient, _apiSuffix)
        {
        }

        public async Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId)
        {
            try
            {
                // Build the full API endpoint URL for the specific resource
                var apiUrl = $"{this.BaseApiUri}/{nameof(GetMyCaptures)}/{userId}";

                // Send the GET request to the API
                var response = await _httpClient.GetAsync(apiUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<Capture>>>(responseJson);

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<IEnumerable<Capture>>.BuildFail("Bad Request.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, build a fail response with the error message
                var failResponse = ResponseFactory<IEnumerable<Capture>>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }
    }
}