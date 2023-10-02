using GeolocationAds.App_ViewModel_Factory;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public class CaptureTemplateViewModel : TemplateBaseViewModel
    {
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

        public CaptureTemplateViewModel(Advertisement advertisement)
        {
            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            this.CurrentAdvertisement = advertisement;

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
    }
}