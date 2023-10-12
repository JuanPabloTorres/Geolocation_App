using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.TemplateViewModel;
using System.Reflection;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.AppTools
{
    public static class AppToolCommon
    {
        //Path Format Root.SubFolder.Subfolder.file
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

        [Obsolete]
        public static async Task<byte[]> ImageSourceToByteArrayAsync(ImageSource imageSource)
        {
            var _completePath = $"GeolocationAds.Resources.Images.{imageSource}";

            if (imageSource is FileImageSource fileImageSource)
            {
                var assem = Assembly.GetExecutingAssembly();

                string resourceName = fileImageSource.File;

                using Stream stream = assem.GetManifestResourceStream(_completePath);

                if (stream != null)
                {
                    using MemoryStream memoryStream = new MemoryStream();

                    await stream.CopyToAsync(memoryStream);

                    return memoryStream.ToArray();
                }
                else
                {
                    throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly.");
                }
            }
            else
            {
                throw new ArgumentException("Unsupported ImageSource type.");
            }
        }

        public static async Task<ContentTypeTemplateViewModel> ProcessContentItem(ContentType item)
        {
            try
            {
                switch (item.Type)
                {
                    case ContentVisualType.Image:
                        return ContentTypeTemplateFactory.BuilContentType(item, item.Content);

                    case ContentVisualType.Video:
                        var file = await CommonsTool.SaveByteArrayToTempFile(item.Content);

                        return ContentTypeTemplateFactory.BuilContentType(item, file);

                    // Add more cases for other content types as needed
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
    }
}