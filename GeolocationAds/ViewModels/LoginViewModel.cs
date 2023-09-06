using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class LoginViewModel : BaseViewModel2<ToolsLibrary.Models.Login, ILoginService>
    {
        private RecoveryPasswordPopUp passwordRecoveryPage;

        private RecoveryPasswordViewModel RecoveryPasswordViewModel;

        public LoginViewModel(ToolsLibrary.Models.Login login, ILoginService service, LogUserPerfilTool logUserPerfil, IForgotPasswordService forgotPasswordService, RecoveryPasswordViewModel recoveryPasswordViewModel) : base(login, service, logUserPerfil)
        {
            LoginCommand = new Command<ToolsLibrary.Models.Login>(VerifyCredential);

            RegisterCommand = new Command(GoToRegister);

            ForgotPasswordCommand = new Command(OpenRecoveryPopUp);

            this.RecoveryPasswordViewModel = recoveryPasswordViewModel;

            this.Model.Username = "test";

            this.Model.Password = "1234";

            WeakReferenceMessenger.Default.Register<LogOffMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    this.LogUserPerfilTool.LogUser = null;

                    await Shell.Current.GoToAsync(nameof(Login));

                    // Manually close the flyout
                    Shell.Current.FlyoutIsPresented = false;
                });
            });

            WeakReferenceMessenger.Default.Register<UpdateMessage<ForgotPassword>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await this.passwordRecoveryPage.CloseAsync();
                });
            });
        }

        public ICommand ForgotPasswordCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        private async void GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(Register));
        }

        private async void OpenRecoveryPopUp()
        {
            this.passwordRecoveryPage = new RecoveryPasswordPopUp(this.RecoveryPasswordViewModel);

            await Shell.Current.CurrentPage.ShowPopupAsync(this.passwordRecoveryPage);
        }

        private async void VerifyCredential(ToolsLibrary.Models.Login credential)
        {
            IsLoading = true;

            try
            {
                var _apiResponse = await this.service.VerifyCredential(credential);

                if (_apiResponse.IsSuccess)
                {
                    if (_apiResponse.Data.UserStatus != ToolsLibrary.Models.UserStatus.ResetPassword)
                    {
                        this.LogUserPerfilTool.LogUser = _apiResponse.Data;

                        WeakReferenceMessenger.Default.Send(new LogInMessage<string>(this.LogUserPerfilTool.LogUser.FullName));

                        await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            IsLoading = false;
        }
    }
}