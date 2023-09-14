using ToolsLibrary.Models;

namespace ToolsLibrary.Factories
{
    public static class ContentTypeFactory
    {
        public static ContentType BuilContentType(byte[] content, ContentVisualType visualType, int? advertisingId, int createdById)
        {
            var _contentType = new ContentType()
            {
                Content = content,
                AdvertisingId = advertisingId.HasValue ? advertisingId.Value : 0,
                Type = visualType,
                CreateDate = DateTime.Now,
                CreateBy = createdById
            };

            return _contentType;
        }
    }
}