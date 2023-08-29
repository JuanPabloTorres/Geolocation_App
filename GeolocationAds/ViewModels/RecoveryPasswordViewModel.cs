using GeolocationAds.Services;
using System.Windows.Input;

namespace GeolocationAds.ViewModels
{
    public partial class RecoveryPasswordViewModel : BaseViewModel
    {
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;

                OnPropertyChanged();
            }
        }

        public ICommand ForgotPasswordCommand { get; set; }

        private IForgotPasswordService service;

        public RecoveryPasswordViewModel(IForgotPasswordService forgotPasswordService)
        {
            this.service = forgotPasswordService;

            ForgotPasswordCommand = new Command(RecoveryPassword);
        }

        public async void RecoveryPassword()
        {
            this.IsLoading = true;

            var _apiResponse = await this.service.RecoveryPassword(this.Email);

            if (_apiResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }

            this.IsLoading = false;
        }
    }
}