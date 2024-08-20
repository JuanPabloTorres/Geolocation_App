using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using ToolsLibrary.Enums;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class LoginViewModel2 : BaseViewModel3<ToolsLibrary.Models.Login, ILoginService>
    {
        private RecoveryPasswordPopUp passwordRecoveryPage;

        private RecoveryPasswordViewModel RecoveryPasswordViewModel;

        private readonly IForgotPasswordService forgotPasswordService;

        private readonly IUserService userService;

        private Providers Provider { get; set; }

        public IAsyncRelayCommand SaveCredentialsCommand => new AsyncRelayCommand(SaveCredentialsAsync);

        public IAsyncRelayCommand ClearCredentialsCommand => new AsyncRelayCommand(ClearCredentialsAsync);

        public IAsyncRelayCommand LoadCredentialsCommand => new AsyncRelayCommand(LoadCredentialsAsync);

        public IAsyncRelayCommand AutoLoginCommand => new AsyncRelayCommand(AutoLoginAsync);

        public readonly string storageNameProvider = nameof(Provider).ToLower();

        [ObservableProperty]
        private bool isRemember;

        public LoginViewModel2(RecoveryPasswordViewModel recoveryPasswordViewModel, ToolsLibrary.Models.Login model, IForgotPasswordService forgotPasswordService, ILoginService service, IUserService userService, LogUserPerfilTool logUserPerfil = null) : base(model, service, logUserPerfil)
        {
            this.RecoveryPasswordViewModel = recoveryPasswordViewModel;

            this.forgotPasswordService = forgotPasswordService;

            this.userService = userService;

            WeakReferenceMessenger.Default.Register<UpdateMessage<ForgotPassword>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await this.passwordRecoveryPage.CloseAsync();

                    this.RecoveryPasswordViewModel = new RecoveryPasswordViewModel(this.forgotPasswordService);
                });
            });

            LoadCredentialsCommand.Execute(null);
        }

        [RelayCommand]
        public async Task SignInWithGoogle()
        {
            var authUrl = new Uri("https://accounts.google.com/o/oauth2/auth?client_id=1077762545698-0qfvitd24opptajm1le5ek72h35ib14s.apps.googleusercontent.com&redirect_uri=com.mycompany.myapp://&response_type=code&scope=openid%20https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile");

            var callbackUrl = new Uri("com.mycompany.myapp://");

            try
            {
                //var authResult = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

                var response = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions()
                {
                    Url = authUrl,
                    CallbackUrl = callbackUrl
                });

                var codeToken = response.Properties["code"];

                var parameters = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string,string>("grant_type","authorization_code"),
                        new KeyValuePair<string,string>("client_id","1077762545698-0qfvitd24opptajm1le5ek72h35ib14s.apps.googleusercontent.com"),
                        new KeyValuePair<string,string>("redirect_uri","com.mycompany.myapp://"),
                        new KeyValuePair<string,string>("code",codeToken),
                });

                HttpClient client = new HttpClient();

                var accessTokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token", parameters);

                if (accessTokenResponse.IsSuccessStatusCode)
                {
                    var data = await accessTokenResponse.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(data);

                    var accessToken = jsonResponse["access_token"]?.ToString();

                    // Llamada a la API de Google para obtener la información del usuario
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        var userInfo = await GetGoogleUserInfo(accessToken);

                        await HandleGoogleSignIn(userInfo);

                        this.Provider = Providers.Google;
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public async Task HandleGoogleSignIn(JObject userInfo)
        {
            try
            {
                var email = userInfo["email"]?.ToString();

                var name = userInfo["name"]?.ToString();

                var googleId = userInfo["id"]?.ToString();

                // Verificar si el usuario ya existe en la base de datos
                var existingUserResponse = await userService.IsEmailRegistered(email);

                ToolsLibrary.Models.Login googleCredential;

                User user;

                if (existingUserResponse.ResponseType == ToolsLibrary.Tools.Type.NotExist)
                {
                    // El usuario no existe, crear un nuevo registro
                    user = new User
                    {
                        CreateBy = 0,
                        CreateDate = DateTime.Now,
                        Email = email,
                        FullName = name,
                        Phone = "111-111-1111",
                        Login = new ToolsLibrary.Models.Login
                        {
                            CreateBy = 0,
                            CreateDate = DateTime.Now,
                            GoogleId = googleId,
                            Password = "google",
                            Username = "googleuser",
                            Provider = ToolsLibrary.Enums.Providers.Google
                        }
                    };

                    var addUserResponse = await userService.Add(user);
                    googleCredential = user.Login;
                }
                else
                {
                    // El usuario ya existe, crear el objeto de credenciales de Google
                    googleCredential = new ToolsLibrary.Models.Login
                    {
                        CreateBy = 0,
                        CreateDate = DateTime.Now,
                        GoogleId = googleId,
                        Password = "google",
                        Username = "googleuser",
                        Provider = ToolsLibrary.Enums.Providers.Google
                    };
                }

                // Generar un token de sesión y manejar la autenticación
                var authResponse = await service.VerifyCredential2(googleCredential);

                if (authResponse.IsSuccess)
                {
                    LogUserPerfilTool.JsonToken = authResponse.Data.JsonToken;

                    LogUserPerfilTool.LogUser = authResponse.Data.LogUser;

                    service.SetJwtToken(LogUserPerfilTool.JsonToken);

                    WeakReferenceMessenger.Default.Send(new LogInMessage<string>(LogUserPerfilTool.LogUser.FullName));

                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

                    await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");
                }
                else
                {
                    // Manejar casos de fallo en la autenticación, si es necesario
                    // Ejemplo: Notificar al usuario o tomar acciones adicionales

                    await CommonsTool.DisplayAlert("Error", authResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

                // Manejo adicional si es necesario
            }
        }

        public async Task<JObject> GetGoogleUserInfo(string accessToken)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetStringAsync("https://www.googleapis.com/oauth2/v2/userinfo");

            return JObject.Parse(response);
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
            this.passwordRecoveryPage = new RecoveryPasswordPopUp(this.RecoveryPasswordViewModel);

            await Shell.Current.CurrentPage.ShowPopupAsync(this.passwordRecoveryPage);
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
                        this.Provider = Providers.App;

                        this.LogUserPerfilTool.JsonToken = _apiResponse.Data.JsonToken;

                        this.LogUserPerfilTool.LogUser = _apiResponse.Data.LogUser;

                        this.service.SetJwtToken(this.LogUserPerfilTool.JsonToken);

                        WeakReferenceMessenger.Default.Send(new LogInMessage<string>(this.LogUserPerfilTool.LogUser.FullName));

                        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

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

        private async Task LoadCredentialsAsync()
        {
            try
            {
                var username = await SecureStorage.GetAsync("username");

                var password = await SecureStorage.GetAsync("password");

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    this.Model.Username = username;

                    this.Model.Password = password;

                    IsRemember = true;

                    await AutoLoginCommand.ExecuteAsync(null);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        private async Task AutoLoginAsync()
        {
            try
            {
                // Obtener el tipo de proveedor desde el almacenamiento seguro
                var providerType = await SecureStorage.GetAsync(storageNameProvider);

                if (!string.IsNullOrWhiteSpace(providerType) && Enum.TryParse(providerType, true, out Providers provider))
                {
                    switch (provider)
                    {
                        case Providers.App:
                            var credential = new ToolsLibrary.Models.Login
                            {
                                Username = this.Model.Username,
                                Password = this.Model.Password
                            };
                            await VerifyCredential(credential);

                            break;

                        case Providers.Google:
                            await SignInWithGoogle();

                            break;

                        // Agregar más proveedores aquí si es necesario
                        default:
                            throw new InvalidOperationException("Unsupported provider type.");
                    }
                }
            }
            catch (ArgumentException ex)
            {
                await CommonsTool.DisplayAlert("Error", "Invalid provider type: " + ex.Message);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }


        partial void OnIsRememberChanged(bool value)
        {
            if (value)
            {
                SaveCredentialsCommand.Execute(null);
            }
            else
            {
                ClearCredentialsCommand.Execute(null);
            }
        }

        private async Task ClearCredentialsAsync()
        {
            SecureStorage.Remove("username");

            SecureStorage.Remove("password");
        }

        private async Task SaveCredentialsAsync()
        {
            try
            {
                await SecureStorage.SetAsync(storageNameProvider, this.Provider.ToString());

                await SecureStorage.SetAsync("username", this.Model.Username);

                await SecureStorage.SetAsync("password", this.Model.Password);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}