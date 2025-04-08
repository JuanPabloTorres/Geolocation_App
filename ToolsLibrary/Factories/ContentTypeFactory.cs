using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace ToolsLibrary.Factories
{
    public static class ContentTypeFactory
    {
        public static ContentType BuilContentType(byte[] content, ContentVisualType visualType, int? advertisingId, int? createdById, string? contentName)
        {
            var _contentType = new ContentType()
            {
                Content = content,
                AdvertisingId = advertisingId.HasValue ? advertisingId.Value : 0,
                Type = visualType,
                CreateDate = DateTime.Now,
                CreateBy = createdById.HasValue ? createdById.Value : 0,
                ContentName = contentName,
                
            };

            return _contentType;
        }

        public static ContentType BuilContentType(byte[] content, ContentVisualType visualType, int? advertisingId, int? createdById, string? contentName, string? filePath)
        {
            var _contentType = new ContentType()
            {
                Content = content,
                AdvertisingId = advertisingId.HasValue ? advertisingId.Value : 0,
                Type = visualType,
                CreateDate = DateTime.Now,
                CreateBy = createdById.HasValue ? createdById.Value : 0,
                ContentName = contentName,
                FilePath = filePath,
                FileSize = CommonsTool.GetFileSize(content)
            };

            return _contentType;
        }

        public static ContentType BuilContentType(string url, ContentVisualType visualType, int? advertisingId, int? createdById, string? contentName, string? filePath)
        {
            var _contentType = new ContentType()
            {
                Content = null,
                AdvertisingId = advertisingId.HasValue ? advertisingId.Value : 0,
                Type = visualType,
                CreateDate = DateTime.Now,
                CreateBy = createdById.HasValue ? createdById.Value : 0,
                ContentName = contentName,
                FilePath=filePath,
                Url = url

            };

            return _contentType;
        }
    }
}