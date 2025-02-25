using Microsoft.Maui.Authentication;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class FacebookAuthService
{
    private readonly HttpClient _httpClient;

    public FacebookAuthService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> SignInWithFacebookAsync()
    {
        try
        {
            string authUrl = GenerateFacebookAuthUrl();

            string redirectUri = "https://com.mycompany.myapp://";

            var authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri(authUrl),
                new Uri(redirectUri));

            // Obtener el `access_token` de la respuesta
            if (authResult.Properties.TryGetValue("access_token", out string accessToken))
            {
                Console.WriteLine($"Access Token: {accessToken}");

                // Obtener información del usuario con el token
                var userInfo = await GetFacebookUserInfoAsync(accessToken);
                return userInfo;
            }

            throw new Exception("No se recibió un token de acceso de Facebook.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en autenticación con Facebook: {ex.Message}");
            return null;
        }
    }

    private async Task<string> GetFacebookUserInfoAsync(string accessToken)
    {
        try
        {
            string graphUrl = $"https://graph.facebook.com/v22.0/me?fields=id,name,email,picture&access_token={accessToken}";

            var response = await _httpClient.GetAsync(graphUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Facebook User Info: {json}");

            return json;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo información del usuario: {ex.Message}");
            return null;
        }
    }

    private string GenerateFacebookAuthUrl()
    {
        string clientId = "2641020766093307";  // Tu App ID de Facebook

        string redirectUri = "https://com.mycompany.myapp://"; // URL de Redirección

        string authUrl = "https://www.facebook.com/v22.0/dialog/oauth";

        return $"{authUrl}?client_id={clientId}" +
               $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
               $"&response_type=token" +
               $"&scope=email,public_profile";
    }
}
