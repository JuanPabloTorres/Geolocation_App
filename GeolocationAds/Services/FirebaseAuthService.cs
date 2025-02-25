using Firebase.Auth;
using Firebase.Auth.Providers;
using GeolocationAds.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly IConfiguration _configuration;

    public FirebaseAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<UserCredential> SignInWithFacebookAsync()
    {
        try
        {
            string clientId = _configuration.GetValue<string>("FacebookSettings:FacebookAppId");

            string redirectUri = _configuration.GetValue<string>("FacebookSettings:FacebookRedirectUri");

            string authUrl = _configuration.GetValue<string>("FacebookSettings:FacebookAuthUrl");

            // 🔹 Incluir un estado único para evitar el error "missing initial state"
            string state = Guid.NewGuid().ToString("N");

            var authUri = new Uri($"{authUrl}?client_id={clientId}" +
                                  $"&redirect_uri={redirectUri}" +
                                  $"&response_type=token" +  // 🔹 Token en lugar de "code"
                                  $"&scope=email,public_profile" +
                                  $"&state={state}"); // 🔹 Agregar estado único

            var callbackUri = new Uri(redirectUri);

            // 🔹 Forzar la apertura en el navegador predeterminado
            //await Browser.Default.OpenAsync(authUri, BrowserLaunchMode.External);

            var response = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions()
            {
                Url = authUri,
                CallbackUrl = callbackUri,
               
            });

            

            if (response.Properties.TryGetValue("access_token", out string accessToken))
            {
                // 🔹 Usar FacebookAuthProvider en lugar de GoogleAuthProvider
                //var credential = Firebase.Auth.Providers.FacebookProvider.GetCredential(accessToken);

                //return await _authClient.SignInWithCredentialAsync(credential);
            }

            throw new Exception("No se recibió un token de acceso de Facebook.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en la autenticación con Facebook: {ex.Message}");
            throw;
        }
    }
}