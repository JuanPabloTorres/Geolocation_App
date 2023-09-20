using CommunityToolkit.Maui.Views;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;

namespace GeolocationAds.TemplateViewModel
{
    public class ContentTypeTemplateViewModel : TemplateBaseViewModel
    {
        private ContentType _contentType;

        public ContentType ContentType
        {
            get => _contentType;

            set
            {
                if (_contentType != value)
                {
                    _contentType = value;

                    OnPropertyChanged();
                }
            }
        }

        private MediaSource _mediaSource;

        public MediaSource MediaSource
        {
            get => _mediaSource;
            set
            {
                if (_mediaSource != value)
                {
                    _mediaSource = value;

                    //SelectedTypeChange(_selectedAdType);

                    OnPropertyChanged();
                }
            }
        }

        public delegate void ApplyQueryAttributesEventHandler(object sender, EventArgs e);

        public static event ApplyQueryAttributesEventHandler ContentTypeDeleted;

        protected virtual void OnDeleteType(EventArgs e)
        {
            ContentTypeDeleted?.Invoke(this, e);
        }

        public ICommand RemoveCommand { get; set; }

        public ContentTypeTemplateViewModel()
        {

            RemoveCommand = new Command(RemoveCurrentItem);
        }

        public override void RemoveCurrentItem()
        {
            OnDeleteType(EventArgs.Empty);
        }
    }
}