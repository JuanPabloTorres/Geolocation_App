using System.Reflection;

namespace GeolocationAds.AppTools
{
    public static class AppToolCommon
    {
        //Path Format Root.SubFolder.Subfolder.file
        public static async Task<byte[]> ImageSourceToByteArrayAsync(string name)
        {

            var _completePath = $"GeolocationAds.Resources.Images.{name}";

            var assem = Assembly.GetExecutingAssembly();

            using var stream = assem.GetManifestResourceStream(_completePath);

            byte[] bytesAvailable = new byte[stream.Length];

            await stream.ReadAsync(bytesAvailable, 0, bytesAvailable.Length);

            return bytesAvailable;
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
    }
}
