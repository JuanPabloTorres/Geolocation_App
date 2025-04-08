using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Pages;
using GeolocationAds.Services;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class UserSettingViewModel : BaseViewModel<User, IUserService>
    {
        private readonly AppShellViewModel2 appShellViewModel2;

        private readonly ILoginService loginService;

        public UserSettingViewModel(User model, IUserService service, LogUserPerfilTool logUserPerfil, AppShellViewModel2 appShellViewModel, ILoginService loginService) : base(model, service, logUserPerfil)
        {
            this.loginService = loginService;

            this.Model = logUserPerfil.LogUser;

            appShellViewModel2 = appShellViewModel;

            RegisterForSignOutMessage();
        }

        [RelayCommand]
        private async Task NavigateEditPerfil(int id)
        {
            await RunWithLoadingIndicator(async () =>
            {
                await Shell.Current.GoToAsync(nameof(EditUserPerfil), new Dictionary<string, object>
                {
                    { "ID", id }
                });
            });
        }

        [RelayCommand]
        private async Task NavigateEditLogin(int id)
        {
            await RunWithLoadingIndicator(async () =>
            {
                await Shell.Current.GoToAsync(nameof(EditLoginCredential), new Dictionary<string, object>
                {
                    { "ID", id }
                });
            });
        }

        [RelayCommand]
        public async Task SignOut()
        {
            await RunWithLoadingIndicator(async () =>
            {
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

                var result = await this.loginService.SignOutAsync(Model.Login);

                if (result.IsSuccess)
                {
                    // Limpia la sesión local
                    LogUserPerfilTool.LogUser = null;

                    LogUserPerfilTool.JsonToken = string.Empty;

                    // Evita que regrese con el botón atrás
                    Application.Current.MainPage = new AppShell(appShellViewModel2);

                    await Shell.Current.GoToAsync(nameof(Login));

                    Shell.Current.FlyoutIsPresented = false;
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", result.Message ?? "No se pudo cerrar sesión.");
                }
            });
        }

        protected async override Task OnSignOutMessageReceivedAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}