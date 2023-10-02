using GeolocationAds.App_ViewModel_Factory;
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

        private ObservableCollection<ContentTypeTemplateViewModel> _contentTypestemplate;

        public ObservableCollection<ContentTypeTemplateViewModel> ContentTypesTemplate
        {
            get => _contentTypestemplate;
            set
            {
                if (_contentTypestemplate != value)
                {
                    _contentTypestemplate = value;

                    OnPropertyChanged();
                }
            }
        }

        public NearByTemplateViewModel(ICaptureService captureService, Advertisement advertisement, LogUserPerfilTool logUser)
        {
            //CaptureCommand = new Command(CaptureItem);

            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            this.LogUser = logUser;

            this.Advertisement = advertisement;

            this.service = captureService;

            CaptureCommand = new Command<Advertisement>(CaptureItem);

            FillTemplate();
        }

        public async void FillTemplate()
        {
            try
            {
                if (!this.Advertisement.Contents.IsObjectNull())
                {
                    foreach (var item in this.Advertisement.Contents)
                    {
                        if (item.Type == ContentVisualType.Image)
                        {
                            var _template = ContentTypeTemplateFactory.BuilContentType(item, item.Content);

                            this.ContentTypesTemplate.Add(_template);
                        }
                        else
                        {
                            var _file = CommonsTool.SaveByteArrayToTempFile(item.Content);

                            var _template = ContentTypeTemplateFactory.BuilContentType(item, _file);

                            this.ContentTypesTemplate.Add(_template);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
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
                    await Shell.Current.DisplayAlert("Notification", "Captured", "OK");
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