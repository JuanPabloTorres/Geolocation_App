using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class SubstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;

            if (!string.IsNullOrEmpty(input) && input.Length >= 1)
            {
                return input.Substring(0, 1);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
