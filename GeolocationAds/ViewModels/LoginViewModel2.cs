using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
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
using ToolsLibrary.Dto;
using ToolsLibrary.Enums;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels;

public partial class LoginViewModel2 : BaseViewModel<ToolsLibrary.Models.Login, ILoginService>
{
    private readonly IContainerLoginServices _containerLoginServices;

    private RecoveryPasswordPopUp _passwordRecoveryPage;

    private Providers Provider { get; set; }

    [ObservableProperty] private bool isRemember;

    public LoginViewModel2(IContainerLoginServices containerLoginServices, AppShellViewModel2 appShellViewModel2) : base(containerLoginServices.LoginModel, containerLoginServices.LoginService, containerLoginServices.LogUserPerfilTool)
    {
        _containerLoginServices = containerLoginServices;

        WeakReferenceMessenger.Default.Register<UpdateMessage<ForgotPassword>>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _passwordRecoveryPage.CloseAsync();

                _containerLoginServices.RecoveryPasswordViewModel = new RecoveryPasswordViewModel(_containerLoginServices.ForgotPasswordService, new NewPasswordDto());
            });
        });

        Task.Run(async () => await AutoLoginAsync());

        RegisterForSignOutMessage();
    }

    /// <summary>
    /// Inicia sesión con Google utilizando el servicio de autenticación.
    /// </summary>
    [RelayCommand]
    private async Task SignInWithGoogle()
    {
        try
        {
            this.IsLoading = true;

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
        finally
        {
            this.IsLoading = false;
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

    /// <summary>
    /// Maneja el proceso de autenticación de Facebook.
    /// </summary>
    public async Task HandleFacebookSignIn(FacebookUserInfoDto userInfo)
    {
        try
        {
            // Verificar si el usuario existe o crearlo
            var facebookCredential = await GetOrCreateFacebookUserAsync(userInfo.Email, userInfo.Name, userInfo.Id);

            // Autenticar al usuario y navegar
            await FacebookAuthenticateAndNavigateAsync(facebookCredential);
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
            var googleLogin = _containerLoginServices.LoginFactory.CreateCredential(googleId, Providers.Google);

            var newUser = _containerLoginServices.UserFactory.CreateUser(email, name, "111-111-1111", googleLogin);

            await _containerLoginServices.UserService.Add(newUser);

            return newUser.Login;
        }
        else
        {
            return _containerLoginServices.LoginFactory.CreateLogin(email, googleId, null, Providers.Google);
        }
    }

    private async Task<ToolsLibrary.Models.Login> GetOrCreateFacebookUserAsync(string email, string name, string facebookId)
    {
        var existingUserResponse = await _containerLoginServices.UserService.IsEmailRegistered(email);

        if (existingUserResponse.ResponseType == ToolsLibrary.Tools.Type.NotExist)
        {
            var facebookLogin = _containerLoginServices.LoginFactory.CreateCredential(facebookId, Providers.Facebook);

            var newUser = _containerLoginServices.UserFactory.CreateUser(email, name, "111-111-1111", facebookLogin);

            await _containerLoginServices.UserService.Add(newUser);

            return newUser.Login;
        }
        else
        {
            return _containerLoginServices.LoginFactory.CreateLogin(email, null, facebookId, Providers.Facebook);
        }
    }

    private async Task AuthenticateAndNavigateAsync(ToolsLibrary.Models.Login credential, Providers provider)
    {
        await RunWithLoadingIndicator(async () =>
        {
            var authResponse = await service.VerifyCredential2(credential);

            if (!authResponse.IsSuccess)
            {
                throw new Exception(authResponse.Message); // Lanza la excepción para que `RunWithLoadingIndicator` la maneje
            }

            //Asigna el ID del proveedor correspondiente
            if (provider == Providers.Google)
                this.Model.GoogleId = credential.GoogleId;
            else if (provider == Providers.Facebook)
                this.Model.FacebookId = credential.FacebookId;
            else if (provider == Providers.App)
                this.Model.ID = credential.ID;

            // Configurar sesión del usuario
            LogUserPerfilTool.JsonToken = authResponse.Data.JsonToken;

            LogUserPerfilTool.LogUser = authResponse.Data.LogUser;

            service.SetJwtToken(LogUserPerfilTool.JsonToken);

            // Configurar la UI de la aplicación
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

            // Verificar si el proveedor cambió y guardar preferencia
            if (this.Provider != provider)
            {
                this.Provider = provider;

                await ConfirmRememberUserAsync();
            }

            //InitializePostLoginViewModels();

            // Notificar inicio de sesión
            WeakReferenceMessenger.Default.Send(new UpdateMessage<User>(LogUserPerfilTool.LogUser));

            // Notificar inicio de sesión
            WeakReferenceMessenger.Default.Send(new UpdateMessage<ToolsLibrary.Models.Login>(LogUserPerfilTool.LogUser.Login));

            // Navegar a la pantalla principal
            await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");
        });
    }

    // Métodos específicos para cada proveedor, llamando al método genérico
    private async Task GoogleAuthenticateAndNavigateAsync(ToolsLibrary.Models.Login googleCredential) =>
        await AuthenticateAndNavigateAsync(googleCredential, Providers.Google);

    private async Task FacebookAuthenticateAndNavigateAsync(ToolsLibrary.Models.Login facebookCredential) =>
        await AuthenticateAndNavigateAsync(facebookCredential, Providers.Facebook);

    private async Task AppAuthenticateAndNavigateAsync(ToolsLibrary.Models.Login appCredential) =>
       await AuthenticateAndNavigateAsync(appCredential, Providers.App);

    private async Task ConfirmRememberUserAsync()
    {
        bool rememberMe = await Shell.Current.DisplayAlert("Remember Me", "Would you like to be remembered for automatic login in the future?", "Yes", "No");

        if (rememberMe)
        {
            await _containerLoginServices.SecureStoreService.SaveAsync(this.Provider, this.Model.Username, this.Model.Password, this.Model.GoogleId, this.Model.FacebookId, rememberMe);
        }
        else
        {
            await _containerLoginServices.SecureStoreService.ClearAll();
        }

        this.IsRemember = rememberMe;
    }

    private async Task AutoLoginAsync()
    {
        //try
        //{
        //}
        //catch (Exception ex)
        //{
        //    await CommonsTool.DisplayAlert("Error", ex.Message);
        //}

        await RunWithLoadingIndicator(async () =>
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
        });
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

                await AppAuthenticateAndNavigateAsync(credential);
                break;

            case Providers.Google:
                var googleCredential = _containerLoginServices.LoginFactory.CreateCredential(await _containerLoginServices.SecureStoreService.GetAsync("googleClientId"), Providers.Google);

                await GoogleAuthenticateAndNavigateAsync(googleCredential);
                break;

            case Providers.Facebook:
                var facebookCredential = _containerLoginServices.LoginFactory.CreateCredential(await _containerLoginServices.SecureStoreService.GetAsync(SecureStoreService.FacebookClientIdKey), Providers.Facebook);

                await FacebookAuthenticateAndNavigateAsync(facebookCredential);
                break;

            default:
                throw new InvalidOperationException("Unsupported provider type.");
        }
    }

     async partial void OnIsRememberChanged(bool value)
    {
        {
            if (value)
            {
                await _containerLoginServices.SecureStoreService.SaveAsync(this.Provider, this.Model.Username, this.Model.Password, this.Model.GoogleId, this.Model.FacebookId, value);
            }
            else
            {
                await _containerLoginServices.SecureStoreService.ClearAll();
            }

            this.IsRemember = value;
        }
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

                WeakReferenceMessenger.Default.Send(new UpdateMessage<User>(LogUserPerfilTool.LogUser));

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
        await RunWithLoadingIndicator(async () =>
        {
            var apiResponse = await service.VerifyCredential2(credential);

            if (!apiResponse.IsSuccess)
            {
                throw new Exception(apiResponse.Message);
            }

            var user = apiResponse.Data.LogUser;

            this.LogUserPerfilTool.JsonToken = apiResponse.Data.JsonToken;

            this.LogUserPerfilTool.LogUser = user;

            this.service.SetJwtToken(this.LogUserPerfilTool.JsonToken);

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

            if (user.UserStatus == ToolsLibrary.Models.UserStatus.ResetPassword)
            {
                throw new Exception(apiResponse.Message);
            }

            if (this.Provider != Providers.App || !IsRemember)
            {
                this.Provider = Providers.App;

                await ConfirmRememberUserAsync();
            }

            WeakReferenceMessenger.Default.Send(new UpdateMessage<User>(LogUserPerfilTool.LogUser));

            // Notificar inicio de sesión
            WeakReferenceMessenger.Default.Send(new UpdateMessage<ToolsLibrary.Models.Login>(LogUserPerfilTool.LogUser.Login));

            await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");
        });
    }

    [RelayCommand]
    private async Task SignInWithFacebook()
    {
        try
        {
            this.IsLoading = true;

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            // 🔹 Pasamos un callback para recibir los datos cuando estén listos
            _containerLoginServices.FacebookAuthWebViewViewModel.OnLoginCompleted = async (userInfo) =>
            {
                if (userInfo != null)
                {
                    // 🔹 Aquí guardamos la sesión o enviamos los datos al backend
                    await HandleFacebookSignIn(userInfo);
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", "⚠️ No se pudo obtener información del usuario.");
                }
            };

            //await Shell.Current.GoToAsync(nameof(FacebookAuthWebViewPage));

            await Shell.Current.Navigation.PushAsync(new FacebookAuthWebViewPage(_containerLoginServices.FacebookAuthWebViewViewModel));
        }
        catch (Exception ex)
        {
            await CommonsTool.DisplayAlert("Error", $"Facebook Sign-in failed: {ex.Message}");
        }
        finally
        {
            this.IsLoading = false;
        }
    }

    protected override async Task OnSignOutMessageReceivedAsync()
    {
        await RunWithLoadingIndicator(async () =>
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            var apiResponse = await this.service.SignOutAsync(this._containerLoginServices.LogUserPerfilTool.LogUser.Login);

            if (!apiResponse.IsSuccess)
            {
                throw new Exception(apiResponse.Message);
            }

            // Limpia la sesión local
            LogUserPerfilTool.LogUser = null;

            LogUserPerfilTool.JsonToken = string.Empty;

            // 🧠 Cancelar mensajes
            //WeakReferenceMessenger.Default.UnregisterAll(this);

            await Task.Delay(5000);

            // Evita que regrese con el botón atrás
            Application.Current.MainPage = new AppShell(this._containerLoginServices.AppShellViewModel);

            await Shell.Current.GoToAsync(nameof(Login));

            Shell.Current.FlyoutIsPresented = false;
        });
    }
}