using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
using GeolocationAds.PopUps;
using GeolocationAds.Services;
using System.Windows.Input;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class LoginViewModel : BaseViewModel2<ToolsLibrary.Models.Login, ILoginService>
    {
        public ICommand LoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public ICommand ForgotPasswordCommand { get; set; }

        public RecoveryPasswordViewModel RecoveryPasswordViewModel;

        public LoginViewModel(ToolsLibrary.Models.Login login, ILoginService service, LogUserPerfilTool logUserPerfil, IForgotPasswordService forgotPasswordService, RecoveryPasswordViewModel recoveryPasswordViewModel) : base(login, service, logUserPerfil)
        {
            LoginCommand = new Command<ToolsLibrary.Models.Login>(VerifyCredential);

            RegisterCommand = new Command(GoToRegister);

            ForgotPasswordCommand = new Command(OpenRecoveryPopUp);

            this.RecoveryPasswordViewModel = recoveryPasswordViewModel;

            this.Model.Username = "Log01";

            this.Model.Password = "12345";

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
        }

        private async void OpenRecoveryPopUp()
        {
            var passwordRecoveryPage = new RecoveryPasswordPopUp(this.RecoveryPasswordViewModel);

            await Application.Current.MainPage.ShowPopupAsync(passwordRecoveryPage);

            //await new TaskFactory().StartNew(() => { Thread.Sleep(5000); });
            //p.Close();

            //this.ShowPopUp()

            //await Shell.Current.CurrentPage.ShowPopupAsync(passwordRecoveryPage);
        }

        private async void VerifyCredential(ToolsLibrary.Models.Login credential)
        {
            IsLoading = true;

            try
            {
                var _apiResponse = await this.service.VerifyCredential(credential);

                if (_apiResponse.IsSuccess)
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
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            IsLoading = false;
        }

        private async void GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(Register));
        }
    }
}