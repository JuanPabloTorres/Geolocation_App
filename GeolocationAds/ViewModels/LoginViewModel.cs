using GeolocationAds.Pages;
using System.Windows.Input;

namespace GeolocationAds.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(VerifyCredential);

            RegisterCommand = new Command(GoToRegister);
        }

        private async void VerifyCredential()
        {
            await Shell.Current.GoToAsync($"///{nameof(SearchAd)}");
        }

        private async void GoToRegister()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Register());
        }
    }
}