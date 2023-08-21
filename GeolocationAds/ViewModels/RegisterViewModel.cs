using GeolocationAds.Services;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel2<User, IUserService>
    {
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;

                    OnPropertyChanged();
                }
            }
        }

        public RegisterViewModel(User user, IUserService userService) : base(user, userService)
        {
            this.User = user;

            this.User.Login = new LoginCredential();
        }
    }
}