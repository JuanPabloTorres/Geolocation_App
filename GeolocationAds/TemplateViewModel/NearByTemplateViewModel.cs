using GeolocationAds.Services;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public class NearByTemplateViewModel : TemplateBaseViewModel
    {
        public ICommand CaptureCommand { get; set; }

        protected readonly ICaptureService service;

        protected readonly LogUserPerfilTool LogUser;

        private Advertisement advertisement;

        public Advertisement Advertisement
        {
            get => advertisement;

            set
            {
                if (advertisement != value)
                {
                    advertisement = value;

                    OnPropertyChanged();
                }
            }
        }

        public NearByTemplateViewModel(ICaptureService captureService, Advertisement advertisement, LogUserPerfilTool logUser)
        {
            //CaptureCommand = new Command(CaptureItem);

            this.LogUser = logUser;

            this.Advertisement = advertisement;

            this.service = captureService;

            CaptureCommand = new Command<Advertisement>(CaptureItem);
        }

        private async void CaptureItem(Advertisement advertisement)
        {
            try
            {
                var _capture = new Capture()
                {
                    AdvertisementId = this.Advertisement.ID,
                    UserId = this.LogUser.GetUserId(),
                    CreateDate = DateTime.Now,
                    CreateBy = this.LogUser.GetUserId(),
                };

                var _apiResponse = await this.service.Add(_capture);

                if (_apiResponse.IsSuccess)
                {
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
    }
}