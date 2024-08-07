using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isExpanded = (bool)value;

            var parameters = ((string)parameter).Split(',');

            return isExpanded ? parameters[1] : parameters[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
