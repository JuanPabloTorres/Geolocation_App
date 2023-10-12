using CommunityToolkit.Maui.Views;
using GeolocationAds.TemplateViewModel;
using ToolsLibrary.Models;

namespace GeolocationAds.App_ViewModel_Factory
{
    public static class ContentTypeTemplateFactory
    {
        public static ContentTypeTemplateViewModel BuilContentType(byte[] content, ContentVisualType visualType, int? advertisingId, int createdById)
        {
            var _contentType = new ContentType()
            {
                Content = content,
                AdvertisingId = advertisingId.HasValue ? advertisingId.Value : 0,
                Type = visualType,
                CreateDate = DateTime.Now,
                CreateBy = createdById
            };

            var _template = new ContentTypeTemplateViewModel()
            {
                ContentType = _contentType,
            };

            return _template;
        }

        public static ContentTypeTemplateViewModel BuilContentType(ContentType contentType, MediaSource mediaSource = null)
        {
            var _template = new ContentTypeTemplateViewModel(contentType, mediaSource);

            return _template;
        }

        public static ContentTypeTemplateViewModel BuilContentType(ContentType contentType, byte[] image)
        {
            var _img = ImageSource.FromStream(() => new MemoryStream(image));

            var _template = new ContentTypeTemplateViewModel(contentType, _img);

            return _template;
        }
    }
}