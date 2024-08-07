
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class LoginService : BaseService<ToolsLibrary.Models.Login>, ILoginService
    {
        public static string _apiSuffix = nameof(LoginService);

        public LoginService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        public async Task<ResponseTool<ToolsLibrary.Models.User>> VerifyCredential(ToolsLibrary.Models.Login credential)
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

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<ToolsLibrary.Models.User>>(responseJson);

                    // Build a success response with the data
                    //var successResponse = ResponseFactory<T>.BuildSusccess("Successfully added.", responseData);

                    //return successResponse;

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<ToolsLibrary.Models.User>.BuildFail("Bad Request.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<ToolsLibrary.Models.User>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public async Task<ResponseTool<LogUserPerfilTool>> VerifyCredential2(ToolsLibrary.Models.Login credential)
        {
            try
            {
                // Convert the data object to JSON
                var json = JsonConvert.SerializeObject(credential);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Build the full API endpoint URL
                var apiUrl = APIPrefix + ApiSuffix;

                // Send the POST request to the API
                var _httpResponse = await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(VerifyCredential2)}", content);

                // Check if the request was successful
                if (_httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await _httpResponse.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<LogUserPerfilTool>>(responseJson);

                    // Build a success response with the data
                    //var successResponse = ResponseFactory<T>.BuildSusccess("Successfully added.", responseData);

                    //return successResponse;

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<LogUserPerfilTool>.BuildFail("Bad Request.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<LogUserPerfilTool>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}