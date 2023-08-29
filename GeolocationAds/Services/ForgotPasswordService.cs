using Newtonsoft.Json;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class ForgotPasswordService : BaseService<ForgotPassword>, IForgotPasswordService
    {
        public static string _apiSuffix = nameof(ForgotPasswordService);

        public ForgotPasswordService()
        { }

        public ForgotPasswordService(string _apiSuffix) : base(_apiSuffix)
        {
        }

        public ForgotPasswordService(HttpClient htppClient, string _apiSuffix) : base(htppClient, _apiSuffix)
        {
        }

        public async Task<ResponseTool<ForgotPassword>> RecoveryPassword(string email)
        {
            try
            {
                // Send the POST request to the API
                var _httpResponse = await _httpClient.GetAsync($"{this.BaseApiUri}/{nameof(RecoveryPassword)}/{email}");

                // Check if the request was successful
                if (_httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await _httpResponse.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<ForgotPassword>>(responseJson);

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<ForgotPassword>.BuildFail("Bad Request", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<ForgotPassword>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }
    }
}