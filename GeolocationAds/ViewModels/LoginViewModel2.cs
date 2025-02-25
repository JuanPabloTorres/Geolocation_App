using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Security;
using System.Net.Http.Headers;
using ToolsLibrary.Enums;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels;

public partial class LoginViewModel2 : BaseViewModel3<ToolsLibrary.Models.Login, ILoginService>
{
    private readonly IContainerLoginServices _containerLoginServices;

    private RecoveryPasswordPopUp _passwordRecoveryPage;

    
    private Providers Provider { get; set; }

    [ObservableProperty]
    private bool isRemember;

    public LoginViewModel2(IContainerLoginServices containerLoginServices) : base(containerLoginServices.LoginModel, containerLoginServices.LoginService, containerLoginServices.LogUserPerfilTool)
    {
        _containerLoginServices = containerLoginServices;

        WeakReferenceMessenger.Default.Register<UpdateMessage<ForgotPassword>>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _passwordRecoveryPage.CloseAsync();
                _containerLoginServices.RecoveryPasswordViewModel = new RecoveryPasswordViewModel(_containerLoginServices.ForgotPasswordService);
            });
        });

      

        Task.Run(async () => await AutoLoginAsync());
    }

    /// <summary>
    /// Inicia sesión con Google utilizando el servicio de autenticación.
    /// </summary>
    [RelayCommand]
    private async Task SignInWithGoogle()
    {
        try
        {
            var accessToken = await _containerLoginServices.GoogleAuthService.AuthenticateAndRetrieveTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var userInfo = await _containerLoginServices.GoogleAuthService.GetGoogleUserInfoAsync(accessToken);

                await HandleGoogleSignIn(userInfo);
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", $"Google Sign-in failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Maneja el proceso de autenticación de Google.
    /// </summary>
    public async Task HandleGoogleSignIn(JObject userInfo)
    {
        try
        {
            var email = userInfo["email"]?.ToString();

            var name = userInfo["name"]?.ToString();

            var googleId = userInfo["id"]?.ToString();

            // Verificar si el usuario existe o crearlo
            var googleCredential = await GetOrCreateGoogleUserAsync(email, name, googleId);

            // Autenticar al usuario y navegar
            await GoogleAuthenticateAndNavigateAsync(googleCredential);
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
    }

    private async Task<ToolsLibrary.Models.Login> GetOrCreateGoogleUserAsync(string email, string name, string googleId)
    {
        var existingUserResponse = await _containerLoginServices.UserService.IsEmailRegistered(email);

        if (existingUserResponse.ResponseType == ToolsLibrary.Tools.Type.NotExist)
        {
            var googleLogin = _containerLoginServices.LoginFactory.CreateLogin(email, googleId);

            var newUser = _containerLoginServices.UserFactory.CreateUser(email, name, "111-111-1111", googleLogin);

            await _containerLoginServices.UserService.Add(newUser);

            return newUser.Login;
        }
        else
        {
            return _containerLoginServices.LoginFactory.CreateLogin(email, googleId, Providers.Google);
        }
    }

    private async Task GoogleAuthenticateAndNavigateAsync(ToolsLibrary.Models.Login googleCredential)
    {
        var authResponse = await service.VerifyCredential2(googleCredential);

        if (authResponse.IsSuccess)
        {
            this.Model.GoogleId = googleCredential.GoogleId;

            LogUserPerfilTool.JsonToken = authResponse.Data.JsonToken;

            LogUserPerfilTool.LogUser = authResponse.Data.LogUser;

            service.SetJwtToken(LogUserPerfilTool.JsonToken);

            WeakReferenceMessenger.Default.Send(new LogInMessage<string>(LogUserPerfilTool.LogUser.FullName));

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

            if (this.Provider != Providers.Google)
            {
                this.Provider = Providers.Google;

                await ConfirmRememberUserAsync();
            }

            await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");
        }
        else
        {
            await CommonsTool.DisplayAlert("Error", authResponse.Message);
        }
    }

    private async Task ConfirmRememberUserAsync()
    {
        bool rememberMe = await Shell.Current.DisplayAlert("Remember Me", "Would you like to be remembered for automatic login in the future?", "Yes", "No");

        if (rememberMe)
        {
            await _containerLoginServices.SecureStoreService.SaveAsync(this.Provider, this.Model.Username, this.Model.Password, this.Model.GoogleId, rememberMe);
        }
        else
        {
            await _containerLoginServices.SecureStoreService.ClearAll();
        }

        this.IsRemember = rememberMe;
    }

    private async Task AutoLoginAsync()
    {
        try
        {
            var isRemember = await _containerLoginServices.SecureStoreService.GetAsync("isRemember");

            if (string.IsNullOrEmpty(isRemember) || isRemember != "True")
            {
                return;
            }

            this.IsRemember = true;

            var providerType = await _containerLoginServices.SecureStoreService.GetAsync("provider");

            if (string.IsNullOrWhiteSpace(providerType))
            {
                throw new ArgumentException("Error", "Provider type is missing.");
            }

            if (Enum.TryParse<Providers>(providerType, true, out var provider))
            {
                this.Provider = provider;

                await HandleProviderLoginAsync(provider);
            }
            else
            {
                throw new ArgumentException("Invalid provider type.");
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
    }

    private async Task HandleProviderLoginAsync(Providers provider)
    {
        switch (provider)
        {
            case Providers.App:
                var username = await _containerLoginServices.SecureStoreService.GetAsync("username");

                var password = await _containerLoginServices.SecureStoreService.GetAsync("password");

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return;
                }

                this.Model.Username = username;

                this.Model.Password = password;

                var credential = new ToolsLibrary.Models.Login
                {
                    Username = this.Model.Username,
                    Password = this.Model.Password
                };

                await AuthenticateUserAsync(credential);
                break;

            case Providers.Google:
                var googleCredential = _containerLoginServices.LoginFactory.CreateGoogleCredential(await _containerLoginServices.SecureStoreService.GetAsync("googleClientId"));

                await GoogleAuthenticateAndNavigateAsync(googleCredential);
                break;

            default:
                throw new InvalidOperationException("Unsupported provider type.");
        }
    }

     partial void OnIsRememberChanged(bool isRemember)
    {
        Task.Run(async () =>
        {
            if (isRemember)
            {
                await _containerLoginServices.SecureStoreService.SaveAsync(this.Provider, this.Model.Username, this.Model.Password, this.Model.GoogleId, isRemember);
            }
            else
            {
                await _containerLoginServices.SecureStoreService.ClearAll();
            }

            this.IsRemember = isRemember;
        });
    }

    private async Task AuthenticateUserAsync(ToolsLibrary.Models.Login credential)
    {
        try
        {
            IsLoading = true;

            var authResponse = await service.VerifyCredential2(credential);

            if (authResponse.IsSuccess)
            {
                LogUserPerfilTool.JsonToken = authResponse.Data.JsonToken;

                LogUserPerfilTool.LogUser = authResponse.Data.LogUser;

                service.SetJwtToken(LogUserPerfilTool.JsonToken);

                WeakReferenceMessenger.Default.Send(new LogInMessage<string>(LogUserPerfilTool.LogUser.FullName));

                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

                if (this.Provider != Providers.App)
                {
                    this.Provider = Providers.App;

                    await ConfirmRememberUserAsync();
                }

                await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");
            }
            else
            {
                await CommonsTool.DisplayAlert("Error", authResponse.Message);
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GoToRegister()
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

        await Shell.Current.GoToAsync(nameof(Register));
    }

    [RelayCommand]
    private async Task OpenRecoveryPopUp()
    {
        this._passwordRecoveryPage = new RecoveryPasswordPopUp(this._containerLoginServices.RecoveryPasswordViewModel);

        await Shell.Current.CurrentPage.ShowPopupAsync(this._passwordRecoveryPage);
    }

    [RelayCommand]
    private async Task VerifyCredential(ToolsLibrary.Models.Login credential)
    {
        try
        {
            IsLoading = true;

            var _apiResponse = await this.service.VerifyCredential2(credential);

            if (!_apiResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");

                return;
            }

            switch (_apiResponse.Data.LogUser.UserStatus)
            {
                case ToolsLibrary.Models.UserStatus.ResetPassword:

                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");

                    break;

                default:

                    this.LogUserPerfilTool.JsonToken = _apiResponse.Data.JsonToken;

                    this.LogUserPerfilTool.LogUser = _apiResponse.Data.LogUser;

                    this.service.SetJwtToken(this.LogUserPerfilTool.JsonToken);

                    WeakReferenceMessenger.Default.Send(new LogInMessage<string>(this.LogUserPerfilTool.LogUser.FullName));

                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

                    if (this.Provider != Providers.App)
                    {
                        this.Provider = Providers.App;

                        await ConfirmRememberUserAsync();
                    }

                    await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");

                    break;
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    //[RelayCommand]
    //private async Task SignInWithFacebook()
    //{
    //    try
    //    {
    //        var authUrl = new Uri($"{_containerLoginServices.Configuration["FacebookSettings:FacebookAuthUrl"]}?" +
    //            $"client_id={_containerLoginServices.Configuration["FacebookSettings:FacebookAppId"]}" +
    //            $"&redirect_uri={_containerLoginServices.Configuration["FacebookSettings:FacebookRedirectUri"]}" +
    //            $"&scope=email,public_profile");

    //        var callbackUrl = new Uri(_containerLoginServices.Configuration["FacebookSettings:FacebookRedirectUri"]);

    //        var result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

    //        if (result.Properties.TryGetValue("access_token", out string accessToken))
    //        {
    //            var userInfo = await GetFacebookUserInfoAsync(accessToken);

    //            //await HandleFacebookSignIn(userInfo);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
    //    }
    //}

    //[RelayCommand]
    //private async Task SignInWithFacebook()
    //{
    //    try
    //    {
    //        // Obtener configuración desde appsettings.json
    //        var facebookAuthUrl = _containerLoginServices.Configuration["FacebookSettings:FacebookAuthUrl"];
    //        var facebookAppId = _containerLoginServices.Configuration["FacebookSettings:FacebookAppId"];
    //        var facebookRedirectUri = _containerLoginServices.Configuration["FacebookSettings:FacebookRedirectUri"];

    //        // Validar que los valores estén configurados correctamente
    //        if (string.IsNullOrWhiteSpace(facebookAuthUrl) || string.IsNullOrWhiteSpace(facebookAppId) || string.IsNullOrWhiteSpace(facebookRedirectUri))
    //        {
    //            throw new InvalidOperationException("Facebook authentication settings are missing or incorrect.");
    //        }

    //        // Construir la URL de autenticación con escape adecuado
    //        var authUrl = new Uri($"{facebookAuthUrl}?" +
    //            $"client_id={facebookAppId}" +
    //            $"&redirect_uri={Uri.EscapeDataString(facebookRedirectUri)}" +
    //            $"&scope=email,public_profile" +
    //            $"&response_type=token");

    //        var callbackUrl = new Uri(facebookRedirectUri);

    //        // Iniciar el proceso de autenticación con Facebook
    //        var result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

    //        // Obtener el token de acceso si la autenticación fue exitosa
    //        if (result.Properties.TryGetValue("access_token", out string accessToken) && !string.IsNullOrEmpty(accessToken))
    //        {
    //            var userInfo = await GetFacebookUserInfoAsync(accessToken);

    //            //await HandleFacebookSignIn(userInfo);
    //        }
    //        else
    //        {
    //            throw new Exception("Facebook authentication failed: Access token was not returned.");
    //        }
    //    }
    //    catch (HttpRequestException httpEx)
    //    {
    //        await CommonsTool.DisplayAlert("Network Error", $"Failed to connect to Facebook: {httpEx.Message}");
    //    }
    //    catch (InvalidOperationException invEx)
    //    {
    //        await CommonsTool.DisplayAlert("Configuration Error", invEx.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
    //    }
    //}

    [RelayCommand]
    private async Task SignInWithFacebook()
    {
        try
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            //await Shell.Current.GoToAsync(nameof(FacebookAuthWebViewPage));

            //var authService = _containerLoginServices.FirebaseAuthService;

            //var authResult = await authService.SignInWithFacebookAsync();

            //if (authResult != null)
            //{
            //    Console.WriteLine($"Inicio de sesión exitoso en Firebase. Usuario: {authResult.User.Info.DisplayName}");

            //    await CommonsTool.DisplayAlert("Exitoso", "entrada exitosa.");

            //    //await Shell.Current.GoToAsync("//MainPage");
            //}
            //else
            //{
            //    throw new Exception("No se pudo autenticar con Firebase.");
            //}

            var tcs = new TaskCompletionSource<string>();

            await Shell.Current.Navigation.PushAsync(new FacebookAuthWebViewPage());

            // Esperar hasta que el WebView devuelva el token de acceso
            var accessToken = await tcs.Task;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var userInfo = await GetFacebookUserInfoAsync(accessToken);

                //await HandleFacebookSignIn(userInfo);
            }
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
        }
    }

    //[RelayCommand]
    //private async Task SignInWithFacebook()
    //{
    //    try
    //    {
    //        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

    //        var tcs = new TaskCompletionSource<string>();

    //        // 🔹 En lugar de Shell.GoToAsync(), usamos PushAsync()
    //        await Shell.Current.Navigation.PushAsync(new FacebookAuthWebViewPage(tcs));

    //        // 🔹 Esperamos la autenticación en el WebView
    //        var accessToken = await tcs.Task;

    //        if (!string.IsNullOrEmpty(accessToken))
    //        {
    //            var userInfo = await GetFacebookUserInfoAsync(accessToken);
    //            //await HandleFacebookSignIn(userInfo);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
    //    }
    //}




    //[RelayCommand]
    //private async Task SignInWithFacebook()
    //{
    //    try
    //    {
    //        var facebookAuthUrl = _containerLoginServices.Configuration["FacebookSettings:FacebookAuthUrl"];

    //        var facebookAppId = _containerLoginServices.Configuration["FacebookSettings:FacebookAppId"];

    //        var facebookRedirectUri = _containerLoginServices.Configuration["FacebookSettings:FacebookRedirectUri"];

    //        if (string.IsNullOrWhiteSpace(facebookAuthUrl) || string.IsNullOrWhiteSpace(facebookAppId) || string.IsNullOrWhiteSpace(facebookRedirectUri))
    //        {
    //            throw new InvalidOperationException("Facebook authentication settings are missing or incorrect.");
    //        }

    //        var facebookRedirectUriEncoded = Uri.EscapeDataString(facebookRedirectUri);

    //        // Construcción de la URL
    //        var authUrl = new Uri($"{facebookAuthUrl}?" +
    //            $"client_id={facebookAppId}" +
    //            $"&redirect_uri={facebookRedirectUriEncoded}" +
    //            $"&scope=email,public_profile" +
    //            $"&response_type=token");

    //        var callbackUrl = new Uri(facebookRedirectUri);

    //        // Iniciar WebAuthenticator
    //        var result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

    //        if (result.Properties.TryGetValue("access_token", out string accessToken) && !string.IsNullOrEmpty(accessToken))
    //        {
    //            //var firebaseToken = await _containerLoginServices.FirebaseAuthService.SignInWithFacebookAsync();

    //            //if (!string.IsNullOrEmpty(firebaseToken))
    //            //{
    //            //    Console.WriteLine("Inicio de sesión exitoso en Firebase. Redirigiendo...");
    //            //    await Shell.Current.GoToAsync("//MainPage");
    //            //}
    //            //else
    //            //{
    //            //    throw new Exception("No se pudo autenticar con Firebase.");
    //            //}
    //        }
    //        else
    //        {
    //            throw new Exception("Facebook authentication failed: Access token was not returned.");
    //        }
    //    }
    //    catch (HttpRequestException httpEx)
    //    {
    //        await CommonsTool.DisplayAlert("Network Error", $"Failed to connect to Facebook: {httpEx.Message}");
    //    }
    //    catch (InvalidOperationException invEx)
    //    {
    //        await CommonsTool.DisplayAlert("Configuration Error", invEx.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
    //    }
    //}



    //[RelayCommand]
    //private async Task SignInWithFacebook()
    //{
    //    try
    //    {
    //        var authService =  _containerLoginServices.FirebaseAuthService;

    //        var authResult = await authService.SignInWithFacebookAsync();

    //        if (authResult != null)
    //        {
    //            Console.WriteLine($"Inicio de sesión exitoso: {authResult.User.Info.DisplayName}");
    //        }
    //        else
    //        {
    //            Console.WriteLine("No se pudo iniciar sesión con Facebook.");
    //        }


    //        // Obtener configuración desde appsettings.json
    //        //var facebookAuthUrl = _containerLoginServices.Configuration["FacebookSettings:FacebookAuthUrl"];
    //        //var facebookAppId = _containerLoginServices.Configuration["FacebookSettings:FacebookAppId"];
    //        //var facebookRedirectUri = _containerLoginServices.Configuration["FacebookSettings:FacebookRedirectUri"];

    //        //// Validar que los valores estén configurados correctamente
    //        //if (string.IsNullOrWhiteSpace(facebookAuthUrl) || string.IsNullOrWhiteSpace(facebookAppId) || string.IsNullOrWhiteSpace(facebookRedirectUri))
    //        //{
    //        //    throw new InvalidOperationException("Facebook authentication settings are missing or incorrect.");
    //        //}

    //        //// Construir la URL de autenticación
    //        //var authUrl = new Uri($"{facebookAuthUrl}?" +
    //        //    $"client_id={facebookAppId}" +
    //        //    $"&redirect_uri={Uri.EscapeDataString(facebookRedirectUri)}" +
    //        //    $"&scope=email,public_profile" +
    //        //    $"&response_type=token");

    //        //var callbackUrl = new Uri(facebookRedirectUri);

    //        //// Iniciar autenticación con WebAuthenticator
    //        //var result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

    //        //// Obtener el token de acceso
    //        //if (result.Properties.TryGetValue("access_token", out string accessToken) && !string.IsNullOrEmpty(accessToken))
    //        //{
    //        //    // Autenticar en Firebase con Facebook
    //        //    var firebaseToken = await _containerLoginServices.FirebaseAuthService.SignInWithFacebookAsync();

    //        //    if (!string.IsNullOrEmpty(firebaseToken))
    //        //    {
    //        //        Console.WriteLine("Inicio de sesión exitoso en Firebase. Redirigiendo...");

    //        //        // Lógica para volver a la pantalla principal
    //        //        await Shell.Current.GoToAsync("//MainPage");
    //        //    }
    //        //    else
    //        //    {
    //        //        throw new Exception("No se pudo autenticar con Firebase.");
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    throw new Exception("Facebook authentication failed: Access token was not returned.");
    //        //}
    //    }
    //    catch (HttpRequestException httpEx)
    //    {
    //        await CommonsTool.DisplayAlert("Network Error", $"Failed to connect to Facebook: {httpEx.Message}");
    //    }
    //    catch (InvalidOperationException invEx)
    //    {
    //        await CommonsTool.DisplayAlert("Configuration Error", invEx.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
    //    }
    //}



    private async Task<JObject> GetFacebookUserInfoAsync(string accessToken)
    {
        try
        {
            HttpClient _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetStringAsync("https://graph.facebook.com/me?fields=id,name,email,picture");

            return JObject.Parse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve Facebook user info: {ex.Message}");

            return null;
        }
    }

}