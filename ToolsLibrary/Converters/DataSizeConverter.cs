using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class DataSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long dataSize)
            {
                if (dataSize >= 1_073_741_824) // GB
                    return $"{dataSize / 1_073_741_824.0:F2} GB";
                else if (dataSize >= 1_048_576) // MB
                    return $"{dataSize / 1_048_576.0:F2} MB";
                else if (dataSize >= 1024) // KB
                    return $"{dataSize / 1024.0:F2} KB";
                else // Bytes
                    return $"{dataSize} B";
            }

            return "0 B";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
