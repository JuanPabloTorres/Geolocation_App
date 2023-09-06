using GeolocationAds.Services;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel2<User, IUserService>
    {
        public RegisterViewModel(User user, IUserService service) : base(user, service)
        {
            this.Model.Login = new ToolsLibrary.Models.Login();

            TestDataDefault();
        }

        private void TestDataDefault()
        {
            this.Model.FullName = "Test 02";

            this.Model.Phone = "787-111-1111";

            this.Model.Email = "est.juanpablotorres@mail.com";

            this.Model.Login.Username = "test";

            this.Model.Login.Password = "12345";
        }
    }
}