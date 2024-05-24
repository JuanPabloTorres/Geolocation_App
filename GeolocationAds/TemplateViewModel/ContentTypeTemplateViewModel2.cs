using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class ContentTypeTemplateViewModel2 : TemplateBaseViewModel2
    {
        [ObservableProperty]
        private ContentType contentType;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private ImageSource image;

        [ObservableProperty]
        private Image image0 = new Image();

        [ObservableProperty]
        private bool isAnimation;

        [ObservableProperty]
        private WebViewSource urlSource;

        private Uri _uri;

        public ContentVisualType ContentVisualType;

        public ContentTypeTemplateViewModel2()
        {
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, MediaSource mediaSource)
        {
            this.ContentType = contentType;

            this.MediaSource = mediaSource;

            ContentVisualType = ContentVisualType.Video;
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, ImageSource imageSource)
        {
            this.ContentType = contentType;

            this.Image = imageSource;

            ContentVisualType = ContentVisualType.Image;
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, Uri url)
        {
            this.ContentType = contentType;

            this.UrlSource = url;

            this._uri = url;

            ContentVisualType = ContentVisualType.URL;
        }

        [RelayCommand]
        public async Task OpenUrl(string url)
        {
            try
            {
                await Browser.Default.OpenAsync(this._uri, BrowserLaunchMode.SystemPreferred);  // Open the URL in the system preferred browser.
            }
            catch (Exception ex)
            {
                // Handle or log exceptions that might occur (e.g., invalid URL format)
                Console.WriteLine($"Could not open URL: {ex.Message}");
            }
        }

        //public async Task InitAnimation()
        //{
        //    IsAnimation = false;

        //    await Task.Delay(1000);  // Delay for 1 second or however long you need

        //    IsAnimation = true;      // Set to true after delay
        //                             // Trigger any necessary PropertyChanged events if using INotifyPropertyChanged
        //}

        public async Task SetAnimation()
        {
            this.IsAnimation = false;

            await Task.Delay(1000);  // Delay for 1 second or however long you need

            this.IsAnimation = true;      // Set to true after delay
        }

        [RelayCommand]
        public override async Task RemoveCurrentItem()
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
    }
}