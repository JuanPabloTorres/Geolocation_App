using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class AdvertisementService : BaseService<Advertisement>, IAdvertisementService
    {
        public AdvertisementService(HttpClient htppClient) : base(htppClient)
        {
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId, int? pageIndex)
        {
            try
            {
                // Build the full API endpoint URL for the "all" endpoint

                // Send an HTTP GET request to the "all" endpoint of your API
                HttpResponseMessage response = await this._httpClient.GetAsync($"{this.BaseApiUri}/{nameof(GetAdvertisementsOfUser)}/{userId}/{typeId}/{pageIndex}");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response content to your custom ResponseTool<IEnumerable<T>> type
                var result = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<Advertisement>>>(responseContent);

                return result;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the request
                // For simplicity, we'll just return an error ResponseTool with the exception message

                var failResponse = ResponseFactory<IEnumerable<Advertisement>>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        {
            try
            {
                using var multipartContent = new MultipartFormDataContent();

                // Add the Advertisement metadata as JSON
                var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8, "application/json"), "advertisementMetadata");

                // Add the contents (images, videos, etc.)
                foreach (var content in data.Contents)
                {
                    if (content.Content != null && content.Content.Length > 0)
                    {
                        var contentStream = new MemoryStream(content.Content);

                        var contentName = content.ContentName ?? "file"; // Default file name if not specified

                        var mediaType = GetMediaType(content.Type);

                        multipartContent.Add(new StreamContent(contentStream), "contents", contentName);
                    }
                }

                // TODO: Add GeolocationAds and Settings if needed, similar to Contents

                // Send the request to the API
                var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);

                if (response.IsSuccessStatusCode)
                {
                    // Handle successful response
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson);

                    return responseData;
                }
                else
                {
                    // Handle failure
                    var failResponse = ResponseFactory<Advertisement>.BuildFail("Bad Request.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                var failResponse = ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        private string GetMediaType(ContentVisualType type)
        {
            return type switch
            {
                ContentVisualType.Image => "image/jpeg", // assuming jpeg, change as necessary
                ContentVisualType.Video => "video/mp4", // assuming mp4, change as necessary
                _ => "application/octet-stream"
            };
        }
    }
}