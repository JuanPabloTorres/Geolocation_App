using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class CaptureService : BaseService<Capture>, ICaptureService
    {
        public static string _apiSuffix = nameof(CaptureService);

        public CaptureService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        //public async Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId, int typeId, int? pageIndex)
        //{
        //    try
        //    {
        //        // Build the full API endpoint URL for the specific resource
        //        var apiUrl = $"{this.BaseApiUri}/{nameof(GetMyCaptures)}/{userId}/{typeId}/{pageIndex}";

        //        // Send the GET request to the API
        //        var response = await _httpClient.GetAsync(apiUrl);

        //        // Check if the request was successful
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Read the response content and deserialize it to the appropriate type T
        //            var responseJson = await response.Content.ReadAsStringAsync();

        //            var responseData = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<Capture>>>(responseJson);

        //            return responseData;
        //        }
        //        else
        //        {
        //            // Build a fail response with the error message from the API
        //            var failResponse = ResponseFactory<IEnumerable<Capture>>.BuildFail("Bad Request.", null);

        //            return failResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // If an exception occurs, build a fail response with the error message
        //        var failResponse = ResponseFactory<IEnumerable<Capture>>.BuildFail($"An error occurred: {ex.Message}", null);

        //        return failResponse;
        //    }
        //}

        public async Task<ResponseTool<IEnumerable<Capture>>> GetMyCaptures(int userId, int typeId, int? pageIndex)
        {
            return await HandleRequest<IEnumerable<Capture>>(async () =>
            {
                var apiUrl = $"{this.BaseApiUri}/{nameof(GetMyCaptures)}/{userId}/{typeId}/{pageIndex}";

                return await _httpClient.GetAsync(apiUrl);
            });
        }

    }
}