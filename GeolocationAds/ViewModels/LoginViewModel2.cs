using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class LoginViewModel2 : BaseViewModel3<ToolsLibrary.Models.Login, ILoginService>
    {
        private RecoveryPasswordPopUp passwordRecoveryPage;

        private RecoveryPasswordViewModel RecoveryPasswordViewModel;

        private readonly IForgotPasswordService forgotPasswordService;

        public IAsyncRelayCommand SaveCredentialsCommand => new AsyncRelayCommand(SaveCredentialsAsync);

        public IAsyncRelayCommand ClearCredentialsCommand => new AsyncRelayCommand(ClearCredentialsAsync);

        public IAsyncRelayCommand LoadCredentialsCommand => new AsyncRelayCommand(LoadCredentialsAsync);

        public IAsyncRelayCommand AutoLoginCommand => new AsyncRelayCommand(AutoLoginAsync);

        [ObservableProperty]
        private bool isRemember;

        public LoginViewModel2(RecoveryPasswordViewModel recoveryPasswordViewModel, ToolsLibrary.Models.Login model, IForgotPasswordService forgotPasswordService, ILoginService service, LogUserPerfilTool logUserPerfil = null) : base(model, service, logUserPerfil)
        {
            this.RecoveryPasswordViewModel = recoveryPasswordViewModel;

            this.forgotPasswordService = forgotPasswordService;

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
            var credential = new ToolsLibrary.Models.Login
            {
                Username = this.Model.Username,
                Password = this.Model.Password
            };
            await VerifyCredential(credential);
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