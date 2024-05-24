using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Dto;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class ForgotPasswordService : BaseService<ForgotPassword>, IForgotPasswordService
    {
        public static string _apiSuffix = nameof(ForgotPasswordService);

        public ForgotPasswordService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        public async Task<ResponseTool<ForgotPassword>> ChangePassword(NewPasswordDto newPasswordDto)
        {
            try
            {
                // Convert the data object to JSON
                var json = JsonConvert.SerializeObject(newPasswordDto);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Build the full API endpoint URL
                var apiUrl = APIPrefix + ApiSuffix;

                // Send the POST request to the API
                var response = await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(ChangePassword)}", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it to the appropriate type T
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<ForgotPassword>>(responseJson);

                    // Build a success response with the data
                    //var successResponse = ResponseFactory<T>.BuildSusccess("Successfully added.", responseData);

                    //return successResponse;

                    return responseData;
                }
                else
                {
                    // Build a fail response with the error message from the API
                    var failResponse = ResponseFactory<ForgotPassword>.BuildFail("Bad Request.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, build a fail response with the error message

                var failResponse = ResponseFactory<ForgotPassword>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        public async Task<ResponseTool<ForgotPassword>> ConfirmCode(string code)
        {
            try
            {
                // Send the POST request to the API
                var _httpResponse = await _httpClient.GetAsync($"{this.BaseApiUri}/{nameof(ConfirmCode)}/{code}");

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