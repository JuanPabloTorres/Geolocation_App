using CommunityToolkit.Maui.Views;
using GeolocationAds.TemplateViewModel;
using ToolsLibrary.Models;

namespace GeolocationAds.App_ViewModel_Factory
{
    public static class ContentTypeTemplateFactory
    {
        public static ContentTypeTemplateViewModel2 BuilContentType(ContentType contentType, MediaSource mediaSource = null)
        {
            var _template = new ContentTypeTemplateViewModel2(contentType, mediaSource);

            return _template;
        }

        public static ContentTypeTemplateViewModel2 BuilContentType(ContentType contentType, byte[] image)
        {
            var _img = ImageSource.FromStream(() => new MemoryStream(image));

            var _template = new ContentTypeTemplateViewModel2(contentType, _img);

            return _template;
        }

        public static ContentTypeTemplateViewModel BuilContentType1(ContentType contentType, MediaSource mediaSource = null)
        {
            var _template = new ContentTypeTemplateViewModel(contentType, mediaSource);

            return _template;
        }

        public static ContentTypeTemplateViewModel BuilContentType1(ContentType contentType, byte[] image)
        {
            var _img = ImageSource.FromStream(() => new MemoryStream(image));

            var _template = new ContentTypeTemplateViewModel(contentType, _img);

            return _template;
        }
    }
}