using GeolocationAds.Pages;
using GeolocationAds.Services;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class UserSettingViewModel : BaseViewModel2<User, IUserService>
    {
        public ICommand onNavigate { get; set; }

        public UserSettingViewModel(User model, IUserService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.Model = logUserPerfil.LogUser;

            this.onNavigate = new Command<int>(Navigate);
        }

        private async void Navigate(int id)
        {
            var navigationParameter = new Dictionary<string, object> { { "ID", id } };

            await Shell.Current.GoToAsync(nameof(EditUserPerfil), navigationParameter);
        }
    }
}