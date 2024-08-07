using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ToolsLibrary.Dto;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class RecoveryPasswordViewModel : BaseViewModel
    {
        public ObservableCollection<ValidationResult> ValidationResults { get; set; }

        private const int StepTotal = 3;

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;

                    OnPropertyChanged();
                }
            }
        }

        private string _code;

        public string Code
        {
            get => _code;
            set
            {
                if (_code != value)
                {
                    _code = value;

                    OnPropertyChanged();
                }
            }
        }

        private NewPasswordDto _newPassword;

        public NewPasswordDto NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;

                    OnPropertyChanged();
                }
            }
        }

        private int _stepIndex;

        public int StepIndex
        {
            get => _stepIndex;
            set
            {
                if (_stepIndex != value)
                {
                    _stepIndex = value;

                    OnPropertyChanged();
                }
            }
        }

        public ICommand ForgotPasswordCommand { get; set; }

        public ICommand ConfirmCodeCommand { get; set; }

        public ICommand SubmitNewPasswordCommand { get; set; }

        public ICommand BackCommand { get; set; }

        public ICommand ForwardCommand { get; set; }

        private IForgotPasswordService service;

        public RecoveryPasswordViewModel(IForgotPasswordService forgotPasswordService)
        {
            this.service = forgotPasswordService;

            this.StepIndex = 1;

            this.NewPassword = new NewPasswordDto();

            ValidationResults = new ObservableCollection<ValidationResult>();

            ForgotPasswordCommand = new Command(RecoveryPassword);

            ConfirmCodeCommand = new Command(ConfirmCode);

            SubmitNewPasswordCommand = new Command(OnSubmit);

            ForwardCommand = new Command(GoForward);

            BackCommand = new Command(GoBack);
        }

        public async void RecoveryPassword()
        {
            this.IsLoading = true;

            var _apiResponse = await this.service.RecoveryPassword(this.Email);

            if (_apiResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");

                this.StepIndex = 2;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }

            this.IsLoading = false;
        }

        public async void ConfirmCode()
        {
            this.IsLoading = true;

            var _apiResponse = await this.service.ConfirmCode(this.Code);

            if (_apiResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");

                this.StepIndex = 3;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }

            this.IsLoading = false;
        }

        public void GoBack()
        {
            this.ValidationResults.Clear();

            if (this.StepIndex > 1)
            {
                this.StepIndex--;
            }
        }

        public void GoForward()
        {
            this.ValidationResults.Clear();

            if (this.StepIndex < StepTotal)
            {
                this.StepIndex++;
            }
        }

        public async void OnSubmit()
        {
            try
            {
                IsLoading = true;

                ValidationResults.Clear();

                this.NewPassword.Code = this.Code;

                var validationContextCurrentType = new ValidationContext(this.NewPassword);

                var isValiteObj = Validator.TryValidateObject(this.NewPassword, validationContextCurrentType, ValidationResults, true);

                if (isValiteObj)
                {
                    var _apiResponse = await this.service.ChangePassword(this.NewPassword);

                    if (_apiResponse.IsSuccess)
                    {
                        await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");

                        WeakReferenceMessenger.Default.Send(new UpdateMessage<ForgotPassword>(_apiResponse.Data));
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}