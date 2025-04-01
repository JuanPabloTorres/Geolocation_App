using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using GeolocationAds.ValidatorsModels;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ToolsLibrary.Attributes.ValidationAttributes;
using ToolsLibrary.Dto;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class RecoveryPasswordViewModel : BaseViewModel<NewPasswordDto, IForgotPasswordService>
    {
        private const int StepTotal = 3;

        [ObservableProperty]      
        public string email;

        [ObservableProperty]
        public string code;

        [ObservableProperty]
        public NewPasswordDto newPassword;

        [ObservableProperty]
        public int stepIndex;

        public RecoveryPasswordViewModel(IForgotPasswordService forgotPasswordService, NewPasswordDto model) : base(model, forgotPasswordService)
        {
            this.service = forgotPasswordService;

            this.StepIndex = 1;

            this.NewPassword = new NewPasswordDto();

            ValidationResults = new ObservableCollection<ValidationResult>();
        }

        [RelayCommand]
        public async Task RecoveryPassword()
        {
            await RunWithLoadingIndicator(async () =>
            {
                ValidationResults.Clear();

                var validatorModel = new EmailValidatorModel { Email = Email };

                var context = new ValidationContext(validatorModel);

                bool isValid = Validator.TryValidateObject(validatorModel, context, ValidationResults, true);

                if (!isValid)
                    return;

                var apiResponse = await service.RecoveryPassword(Email);

                if (!apiResponse.IsSuccess)
                    throw new Exception(apiResponse.Message);

                await Shell.Current.DisplayAlert("Notification", apiResponse.Message, "OK");

                StepIndex = 2;
            });
        }

        [RelayCommand]
        public async Task ConfirmCode()
        {
          

            await RunWithLoadingIndicator(async () =>
            {
                ValidationResults.Clear();

                var validatorModel = new CodeValidatorModel { Code = Code };

                var context = new ValidationContext(validatorModel);

                bool isValid = Validator.TryValidateObject(validatorModel, context, ValidationResults, true);

                if (!isValid)
                    return;

                var apiResponse = await service.ConfirmCode(Code);

                if (!apiResponse.IsSuccess)
                    throw new Exception(apiResponse.Message);

                await Shell.Current.DisplayAlert("Notification", apiResponse.Message, "OK");

                StepIndex = 3;
            });
        }

        [RelayCommand]
        public void GoBack()
        {
            this.ValidationResults.Clear();

            if (this.StepIndex > 1)
            {
                this.StepIndex--;
            }
        }

        [RelayCommand]
        public void GoForward()
        {
            this.ValidationResults.Clear();

            if (this.StepIndex < StepTotal)
            {
                this.StepIndex++;
            }
        }

        public override async Task Submit(NewPasswordDto model)
        {
            await RunWithLoadingIndicator(async () =>
            {
                ValidationResults.Clear();

                NewPassword.Code = Code;

                var validationContext = new ValidationContext(model);

                bool isValid = Validator.TryValidateObject(model, validationContext, ValidationResults, true);

                if (!isValid)
                    return;

                var apiResponse = await service.ChangePassword(model);

                if (!apiResponse.IsSuccess)
                {
                    throw new Exception(apiResponse.Message);
                }

                await Shell.Current.DisplayAlert("Notification", apiResponse.Message, "OK");

                WeakReferenceMessenger.Default.Send(new UpdateMessage<ForgotPassword>(apiResponse.Data));
            });
        }
    }
}