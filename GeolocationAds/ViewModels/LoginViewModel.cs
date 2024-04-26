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

            this.Model.Username = "test01";

            this.Model.Password = "12345";

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
            try
            {
                IsLoading = true;

                var _apiResponse = await this.service.VerifyCredential2(credential);

                if (_apiResponse.IsSuccess)
                {
                    if (_apiResponse.Data.LogUser.UserStatus != ToolsLibrary.Models.UserStatus.ResetPassword)
                    {
                        this.LogUserPerfilTool.JsonToken = _apiResponse.Data.JsonToken;

                        this.LogUserPerfilTool.LogUser = _apiResponse.Data.LogUser;

                        this.service.SetJwtToken(this.LogUserPerfilTool.JsonToken);

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
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}