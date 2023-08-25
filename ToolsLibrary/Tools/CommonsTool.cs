using System.Reflection;

namespace ToolsLibrary.Tools
{
    public static class CommonsTool
    {
        public static async Task<byte[]> GetFileBytesAsync(FileResult fileResult)
        {
            if (fileResult != null)
            {
                using (var stream = await fileResult.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        return ms.ToArray();
                    }
                }
            }
            return null;
        }

        public static async Task<byte[]> ImageSourceToByteArrayAsync()
        {

            var assem = Assembly.GetExecutingAssembly();

            using var stream = assem.GetManifestResourceStream("GeolocationAds.Resources.Images.mediacontent.png");

            byte[] bytesAvailable = new byte[stream.Length];

            await stream.ReadAsync(bytesAvailable, 0, bytesAvailable.Length);

            return bytesAvailable;
        }

        public static async Task<byte[]> ImageSourceToByteArrayAsync(ImageSource imageSource)
        {
            if (imageSource is FileImageSource fileImageSource)
            {
                var assem = Assembly.GetExecutingAssembly();

                string resourceName = fileImageSource.File;

                using Stream stream = assem.GetManifestResourceStream(resourceName);

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
    }
}
