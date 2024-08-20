using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class DataSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long dataSize)
            {
                const long OneKB = 1024;

                const long OneMB = OneKB * 1024;

                const long OneGB = OneMB * 1024;

                if (dataSize >= OneGB) // GB
                    return $"{dataSize / (double)OneGB:F2} GB";
                else if (dataSize >= OneMB) // MB
                    return $"{dataSize / (double)OneMB:F2} MB";
                else if (dataSize >= OneKB) // KB
                    return $"{dataSize / (double)OneKB:F2} KB";
                else // Bytes
                    return string.Empty;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
