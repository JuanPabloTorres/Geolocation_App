using GeolocationAds.AppTools;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
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

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; }

        public NearByTemplateViewModel(ICaptureService captureService, Advertisement advertisement, LogUserPerfilTool logUser)
        {
            this.LogUser = logUser;

            this.Advertisement = advertisement;

            this.service = captureService;

            CaptureCommand = new Command<Advertisement>(CaptureItem);
        }

        public async Task InitializeAsync()
        {
            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel2>();

            await FillTemplate();
        }

        public async Task FillTemplate()
        {
            try
            {
                if (!this.Advertisement.Contents.IsObjectNull())
                {
                    foreach (var item in this.Advertisement.Contents)
                    {
                        var _template = await AppToolCommon.ProcessContentItem(item);

                        this.ContentTypesTemplate.Add(_template);
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
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
                    await CommonsTool.DisplayAlert("Notification", "Capture completed successfully.");
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}