using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ToolsLibrary.Models;

using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class ContentTypeTemplateViewModel : TemplateBaseViewModel<ContentTypeTemplateViewModel>
    {
        [ObservableProperty]
        private ContentType contentType;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private ImageSource image;

       

        protected virtual void OnDeleteType()
        {
            ItemDeleted?.Invoke(this);
        }

        public ContentTypeTemplateViewModel(Action<ContentTypeTemplateViewModel> onDelete)
        {
            //RemoveCommand = new Command(RemoveCurrentItem);

            ItemDeleted = onDelete;
        }

        public ContentTypeTemplateViewModel(ContentType contentType, MediaSource mediaSource, Action<ContentTypeTemplateViewModel> onDelete)
        {
            this.ContentType = contentType;

            this.MediaSource = mediaSource;

            ItemDeleted = onDelete;

            //RemoveCommand = new Command(RemoveCurrentItem);
        }

        public ContentTypeTemplateViewModel(ContentType contentType, ImageSource imageSource, Action<ContentTypeTemplateViewModel> onDelete)
        {
            this.ContentType = contentType;

            this.Image = imageSource;

            ItemDeleted = onDelete;

            //RemoveCommand = new Command(RemoveCurrentItem);
        }

        public override async Task RemoveCurrentItem()
        {
            try
            {
                this.IsLoading = true;

                OnDeleteType();
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
    }
}