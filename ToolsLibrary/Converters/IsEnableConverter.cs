using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class IsEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool isEnable = (bool)value;

                if (isEnable)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}