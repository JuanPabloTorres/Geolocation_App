using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class GeolocationAdService : BaseService<GeolocationAd>, IGeolocationAdService
    {
        public static string _apiSuffix = nameof(GeolocationAd);

        public GeolocationAdService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        //public async Task<ResponseTool<IEnumerable<GeolocationAd>>> FindAdsNearby(CurrentLocation currentLocation, string distance, int settinTypeId)
        //{
        //    try
        //    {
        //        // Convert the data object to JSON
        //        var json = JsonConvert.SerializeObject(currentLocation);

        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        // Build the full API endpoint URL
        //        var apiUrl = APIPrefix + ApiSuffix;

        //        // Send the POST request to the API
        //        var response = await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(FindAdsNearby)}/{distance}/{settinTypeId}", content);

        //        // Check if the request was successful
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Read the response content and deserialize it to the appropriate type T
        //            var responseJson = await response.Content.ReadAsStringAsync();

        //            var responseData = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<GeolocationAd>>>(responseJson);

        //            // Build a success response with the data
        //            //var successResponse = ResponseFactory<T>.BuildSusccess("Successfully added.", responseData);

        //            //return successResponse;

        //            return responseData;
        //        }
        //        else
        //        {
        //            // Build a fail response with the error message from the API
        //            var failResponse = ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail("Request Error.", null);

        //            return failResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // If an exception occurs, build a fail response with the error message

        //        var failResponse = ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail($"An error occurred: {ex.Message}", null);

        //        return failResponse;
        //    }
        //}

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> FindAdsNearby(CurrentLocation currentLocation, string distance, int settinTypeId)
        {
            return await HandleRequest<IEnumerable<GeolocationAd>>(async () =>
            {
                var json = JsonConvert.SerializeObject(currentLocation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{this.BaseApiUri}/{nameof(FindAdsNearby)}/{distance}/{settinTypeId}";

                return await _httpClient.PostAsync(url, content);
            });
        }


        //public async Task<ResponseTool<IEnumerable<Advertisement>>> FindAdNear2(CurrentLocation currentLocation, string distance, int settinTypeId, int? pageIndex = 1)
        //{
        //    try
        //    {
        //        // Convert the data object to JSON
        //        var json = JsonConvert.SerializeObject(currentLocation);

        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        // Build the full API endpoint URL
        //        var apiUrl = APIPrefix + ApiSuffix;

        //        // Send the POST request to the API
        //        var response = await this._httpClient.PostAsync($"{this.BaseApiUri}/{nameof(FindAdNear2)}/{distance}/{settinTypeId}/{pageIndex}", content);

        //        // Check if the request was successful
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Read the response content and deserialize it to the appropriate type T
        //            var responseJson = await response.Content.ReadAsStringAsync();

        //            var responseData = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<Advertisement>>>(responseJson);

        //            return responseData;
        //        }
        //        else
        //        {
        //            // Build a fail response with the error message from the API
        //            var failResponse = ResponseFactory<IEnumerable<Advertisement>>.BuildFail("Request Error.", null);

        //            return failResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var failResponse = ResponseFactory<IEnumerable<Advertisement>>.BuildFail($"An error occurred: {ex.Message}", null);

        //        return failResponse;
        //    }
        //}

        public async Task<ResponseTool<IEnumerable<Advertisement>>> FindAdNear2(CurrentLocation currentLocation, string distance, int settinTypeId, int? pageIndex = 1)
        {
            return await HandleRequest<IEnumerable<Advertisement>>(async () =>
            {
                var json = JsonConvert.SerializeObject(currentLocation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{this.BaseApiUri}/{nameof(FindAdNear2)}/{distance}/{settinTypeId}/{pageIndex}";

                return await _httpClient.PostAsync(url, content);
            });
        }

    }
}