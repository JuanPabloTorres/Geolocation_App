using CommunityToolkit.Maui.Views;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

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

        public event ApplyQueryAttributesEventHandler ContentTypeDeleted;

        protected virtual void OnDeleteType(EventArgs e)
        {
            ContentTypeDeleted?.Invoke(this, e);
        }

        public ContentTypeTemplateViewModel()
        {
            RemoveCommand = new Command(RemoveCurrentItem);
        }

        public ContentTypeTemplateViewModel(ContentType contentType, MediaSource mediaSource)
        {
            this.ContentType = contentType;

            this.MediaSource = mediaSource;

            RemoveCommand = new Command(RemoveCurrentItem);
        }

        public ContentTypeTemplateViewModel(ContentType contentType, ImageSource imageSource)
        {
            this.ContentType = contentType;

            this.Image = imageSource;

            RemoveCommand = new Command(RemoveCurrentItem);
        }

        public override async void RemoveCurrentItem()
        {
            try
            {
                this.IsLoading = true;

                OnDeleteType(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        //public void Dispose()
        //{
        //    this.Image = null;

        //    this.MediaSource = null;

        //    this.ContentType = null;
        //}
    }
}