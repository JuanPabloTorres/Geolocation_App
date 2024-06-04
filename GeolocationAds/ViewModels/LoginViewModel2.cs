using CommunityToolkit.Maui.Views;
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

        public LoginViewModel2(RecoveryPasswordViewModel recoveryPasswordViewModel, ToolsLibrary.Models.Login model, IForgotPasswordService forgotPasswordService, ILoginService service, LogUserPerfilTool logUserPerfil = null) : base(model, service, logUserPerfil)
        {
            this.RecoveryPasswordViewModel = recoveryPasswordViewModel;

            this.forgotPasswordService = forgotPasswordService;

            this.Model.Username = "test";

            this.Model.Password = "123";

            WeakReferenceMessenger.Default.Register<UpdateMessage<ForgotPassword>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await this.passwordRecoveryPage.CloseAsync();

                    this.RecoveryPasswordViewModel = new RecoveryPasswordViewModel(this.forgotPasswordService);
                });
            });
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
    }
}