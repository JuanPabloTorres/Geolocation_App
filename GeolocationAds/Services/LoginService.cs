using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class LoginService : BaseService<ToolsLibrary.Models.Login>, ILoginService
    {
        public static string _apiSuffix = nameof(LoginService);

        public LoginService()
        { }

        public LoginService(string _apiSuffix) : base(_apiSuffix)
        {
        }

        public LoginService(HttpClient htppClient, string _apiSuffix) : base(htppClient, _apiSuffix)
        {
        }

        public async Task<ResponseTool<ToolsLibrary.Models.Login>> VerifyCredential(ToolsLibrary.Models.Login credential)
        {
            try
            {
                // Convert the data object to JSON
                var json = JsonConvert.SerializeObject(credential);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Build the full API endpoint URL
                var apiUrl = APIPrefix + ApiSuffix;

                // Send the POST request to the API
                var _httpResponse = await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(VerifyCredential)}", content);

                // Check if the request was successful
                if (_httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await _httpResponse.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<ToolsLibrary.Models.Login>>(responseJson);

                    // Build a success response with the data
                    //var successResponse = ResponseFactory<T>.BuildSusccess("Successfully added.", responseData);

                    //return successResponse;

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<ToolsLibrary.Models.Login>.BuildFail("Failed to add.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<ToolsLibrary.Models.Login>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}