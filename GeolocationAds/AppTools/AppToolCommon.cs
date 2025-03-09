using CommunityToolkit.Maui.Views;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System.Reflection;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.AppTools
{
    public static class AppToolCommon
    {
        public static async Task<byte[]> ImageSourceToByteArrayAsync(string name)
        {
            try
            {
                var _completePath = $"GeolocationAds.Resources.Images.{name}";

                var assem = Assembly.GetExecutingAssembly();

                using var stream = assem.GetManifestResourceStream(_completePath);

                if (stream.IsObjectNull())
                {
                    throw new Exception($"Resource '{_completePath}' not found.");
                }

                byte[] bytesAvailable = new byte[stream.Length];

                await stream.ReadAsync(bytesAvailable, 0, bytesAvailable.Length);

                return bytesAvailable;
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

                return null;
            }
        }

        public static Stream GetEmbeddedResourceStream(string resourceFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"GeolocationAds.Resources.Images.{resourceFileName}";

            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
                throw new FileNotFoundException($"Resource not found: {resourceFileName}", resourceName);
            return resourceStream;
        }

        public static async Task<ContentTypeTemplateViewModel2> ProcessContentItem(ContentType item, IAdvertisementService advertisementService)
        {
            try
            {
                switch (item.Type)
                {
                    case ContentVisualType.Image:
                        return ContentTypeTemplateFactory.BuilContentType(item, item.Content);

                    case ContentVisualType.URL:

                        var _url = new Uri(item.Url);

                        return ContentTypeTemplateFactory.BuilContentType(item, _url);

                    case ContentVisualType.Video:

                        var streamingResponse = await advertisementService.GetStreamingVideoUrl(item.ID);

                        if (!streamingResponse.IsSuccess)
                        {
                            await CommonsTool.DisplayAlert("Error", streamingResponse.Message);

                            return null;
                        }

                        return ContentTypeTemplateFactory.BuilContentType(item, streamingResponse.Data);

                    default:
                        // Handle other content types or provide a default action.
                        return null;
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

                return null;
            }
        }

        public static ImageSource LoadImageFromBytes(byte[] imageBytes)
        {
            //Immediately create, use, and dispose the stream within this scope
            using (var imageStream = new MemoryStream(imageBytes))
            {
                return ImageSource.FromStream(() => new MemoryStream(imageStream.ToArray()));
            }
        }

        public static async Task<ContentTypeTemplateViewModel2> GetDefaultContentTypeTemplateViewModel(int? userId)
        {
            try
            {
                var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(ConstantsTools.FILENAME);

                if (!_defaulMedia.IsObjectNull())
                {
                    var _content = ContentTypeFactory.BuilContentType(_defaulMedia, ContentVisualType.Image, null, userId, ConstantsTools.FILENAME);

                    _content.ContentName = ConstantsTools.FILENAME;

                    var _template = ContentTypeTemplateFactory.BuilContentType(_content, _content.Content);

                    return _template;
                }

                return null;
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

                return null;
            }
        }

        public static async Task<ContentType> GetDefaultContentType(int? userId)
        {
            try
            {
                var _defaulMedia = await AppToolCommon.ImageSourceToByteArrayAsync(ConstantsTools.FILENAME);

                if (!_defaulMedia.IsObjectNull())
                {
                    var _content = ContentTypeFactory.BuilContentType(_defaulMedia, ContentVisualType.Image, null, userId, ConstantsTools.FILENAME);

                    _content.ContentName = ConstantsTools.FILENAME;

                    return _content;
                }

                return null;
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

                return null;
            }
        }

        public static string FormatPhoneNumber(string input)
        {
            string digits = new string(input.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(digits)) return string.Empty;

            return digits.Length switch
            {
                <= 3 => $"({digits}",
                <= 6 => $"({digits[..3]})-{digits[3..]}",
                _ => $"({digits[..3]})-{digits[3..6]}-{digits[6..]}"
            };
        }
    }
}