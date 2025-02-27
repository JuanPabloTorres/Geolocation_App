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
        //public ICommand onNavigateEditPerfilCommand { get; set; }

        //public ICommand onNavigateEditLoginCommand { get; set; }

        public UserSettingViewModel(User model, IUserService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.Model = logUserPerfil.LogUser;

            //this.onNavigateEditPerfilCommand = new Command<int>(NavigateEditPerfil);

            //this.onNavigateEditLoginCommand = new Command<int>(NavigateEditLogin);
        }

        [RelayCommand]
        private async Task NavigateEditPerfil(int id)
        {
            var navigationParameter = new Dictionary<string, object> { { "ID", id } };

            await Shell.Current.GoToAsync(nameof(EditUserPerfil), navigationParameter);
        }
        [RelayCommand]
        private async Task NavigateEditLogin(int id)
        {
            var navigationParameter = new Dictionary<string, object> { { "ID", id } };

            await Shell.Current.GoToAsync(nameof(EditLoginCredential), navigationParameter);
        }

    }
}