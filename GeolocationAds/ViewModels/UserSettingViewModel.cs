using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Pages;
using GeolocationAds.Services;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class UserSettingViewModel : BaseViewModel3<User, IUserService>
    {
        public UserSettingViewModel(User model, IUserService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.Model = logUserPerfil.LogUser;
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
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            await Shell.Current.GoToAsync(nameof(Login));

            Shell.Current.FlyoutIsPresented = false;
        }
    }
}