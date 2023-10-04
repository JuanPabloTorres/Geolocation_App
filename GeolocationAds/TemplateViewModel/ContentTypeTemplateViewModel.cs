using CommunityToolkit.Maui.Views;
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

        private ImageSource _image;

        public ImageSource Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;

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

        public ContentTypeTemplateViewModel()
        {
            RemoveCommand = new Command(RemoveCurrentItem);
        }

        public override void RemoveCurrentItem()
        {
            OnDeleteType(EventArgs.Empty);
        }

        //public void Dispose()
        //{
        //    this.Image = null;

        //    this.MediaSource = null;

        //    this.ContentType = null;
        //}
    }
}