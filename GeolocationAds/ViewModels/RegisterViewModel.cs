using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel3<User, IUserService>
    {
        [ObservableProperty]
        private User _newUser = new User();

        public RegisterViewModel(User user, IUserService service) : base(user, service)
        {
            NewUser = new User();

            TestDataDefault();
        }

        private void TestDataDefault()
        {
            this.Model.FullName = "Test";

            this.Model.Phone = "111-111-1111";

            this.Model.Email = "test@gmail.com";

            this.Model.Login = new ToolsLibrary.Models.Login()
            {
                Username = "test",
                Password = "12345"
            };
        }

        [RelayCommand]
        private void ClearText()
        {
            ClearData();
        }

        private void ClearData()
        {
            this.Model.FullName = string.Empty;

            this.Model.Phone = string.Empty;

            this.Model.Email = string.Empty;

            this.Model.Login.Username = string.Empty;

            this.Model.Login.Password = string.Empty;
        }
    }
}