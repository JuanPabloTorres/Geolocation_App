using GeolocationAds.Pages;
using GeolocationAds.Services;
using System.Windows.Input;

namespace GeolocationAds.ViewModels
{
    public partial class LoginViewModel : BaseViewModel2<ToolsLibrary.Models.Login, ILoginService>
    {
        public ICommand LoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public LoginViewModel(ToolsLibrary.Models.Login login, ILoginService service) : base(login, service)
        {
            LoginCommand = new Command<ToolsLibrary.Models.Login>(VerifyCredential);

            RegisterCommand = new Command(GoToRegister);
        }

        private async void VerifyCredential(ToolsLibrary.Models.Login credential)
        {
            try
            {
                var _apiResponse = await this.service.VerifyCredential(credential);

                if (_apiResponse.IsSuccess)
                {
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


        }

        private async void GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(Register));
        }
    }
}