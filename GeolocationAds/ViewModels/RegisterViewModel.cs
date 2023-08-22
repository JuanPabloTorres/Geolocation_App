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

        public RegisterViewModel(User user, IUserService service) : base(user, service)
        {
            this.User = user;

            this.User.Login = new ToolsLibrary.Models.Login();

            TestDataDefault();
        }

        private void TestDataDefault()
        {
            this.User.FullName = "User 02";

            this.User.Phone = "787-111-1111";

            this.User.Email = "user02@mail.com";

            this.User.Login.Username = "user02";

            this.User.Login.Password = "12345";
        }
    }
}