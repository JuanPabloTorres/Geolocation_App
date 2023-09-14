using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class ByteArrayToVideSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                var Video = new HtmlWebViewSource
                {
                    Html = $"<html><body><video width='100%' height='100%' controls><source src='data:video/mp4;base64,{System.Convert.ToBase64String(byteArray)}' type='video/mp4' /></video></body></html>"
                };

                return Video;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is byte[] byteArray && byteArray.Length > 0)
            //{
            //    return ImageSource.FromStream(() => new MemoryStream(byteArray));
            //}

            //return null;

            //var _fileName = "mediacontent.png";

            //return ImageSource.FromResource($"GeolocationAds.Resources.Images.{_fileName}");

            return null;
        }
    }
}
