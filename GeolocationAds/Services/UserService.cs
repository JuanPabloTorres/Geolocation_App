using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public static string _apiSuffix = nameof(UserService);

        public UserService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        public async Task<ResponseTool<bool>> IsEmailRegistered(string email)
        {
            try
            {
                // Enviar la solicitud GET a la API
                var response = await _httpClient.GetAsync($"{this.BaseApiUri}/{nameof(IsEmailRegistered)}/{email}");

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta y deserializarlo al tipo ResponseTool<bool>
                    var responseData = JsonConvert.DeserializeObject<ResponseTool<bool>>(await response.Content.ReadAsStringAsync());

                    return responseData ?? ResponseFactory<bool>.BuildFail("No data returned.", false);
                }

                // Construir una respuesta de fallo si el estado de la solicitud no es exitoso
                return ResponseFactory<bool>.BuildFail($"API request failed with status code: {response.StatusCode}.", false);
            }
            catch (HttpRequestException httpEx)
            {
                // Manejar excepciones específicas de solicitudes HTTP
                return ResponseFactory<bool>.BuildFail($"HTTP Request Error: {httpEx.Message}", false, ToolsLibrary.Tools.Type.Exception);
            }
            catch (JsonException jsonEx)
            {
                // Manejar excepciones de deserialización JSON
                return ResponseFactory<bool>.BuildFail($"JSON Deserialization Error: {jsonEx.Message}", false, ToolsLibrary.Tools.Type.Exception);
            }
            catch (Exception ex)
            {
                // Manejar cualquier otra excepción genérica
                return ResponseFactory<bool>.BuildFail($"Unexpected Error: {ex.Message}", false, ToolsLibrary.Tools.Type.Exception);
            }
        }

    }
}