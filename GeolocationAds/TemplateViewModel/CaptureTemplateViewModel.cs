using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public class CaptureTemplateViewModel : TemplateBaseViewModel
    {
        public ICaptureService Service { get; set; }

        public ObservableCollection<ContentTypeTemplateViewModel> ContentTypesTemplate
        {
            get; set;
        }

        private Advertisement _currentAdvertisement;

        public Advertisement CurrentAdvertisement
        {
            get => _currentAdvertisement;
            set
            {
                if (_currentAdvertisement != value)
                {
                    _currentAdvertisement = value;

                    OnPropertyChanged();
                }
            }
        }

        private Capture _capture;

        public Capture Capture
        {
            get => _capture;
            set
            {
                if (_capture != value)
                {
                    _capture = value;

                    OnPropertyChanged();
                }
            }
        }

        public CaptureTemplateViewModel(Capture capture, ICaptureService service)
        {
            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            this.RemoveCommand = new Command<Capture>(Remove);

            this.Capture = capture;

            this.CurrentAdvertisement = capture.Advertisements;

            this.Service = service;

            FillTemplate();
        }

        public void FillTemplate()
        {
            if (!this.CurrentAdvertisement.Contents.IsObjectNull())
            {
                foreach (var item in this.CurrentAdvertisement.Contents)
                {
                    if (item.Type == ContentVisualType.Image)
                    {
                        var _template = ContentTypeTemplateFactory.BuilContentType(item, item.Content);

                        this.ContentTypesTemplate.Add(_template);
                    }

                    if (item.Type == ContentVisualType.Video)
                    {
                        var _file = CommonsTool.SaveByteArrayToTempFile(item.Content);

                        var _template = ContentTypeTemplateFactory.BuilContentType(item, _file);

                        this.ContentTypesTemplate.Add(_template);
                    }
                }
            }
        }

        public async void Remove(Capture capture)
        {
            try
            {
                var _apiResponse = await this.Service.Remove(capture.ID);

                if (_apiResponse.IsSuccess)
                {
                    OnDeleteItem(EventArgs.Empty);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}