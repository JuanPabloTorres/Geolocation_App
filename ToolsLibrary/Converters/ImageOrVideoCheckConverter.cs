using System.Globalization;
using ToolsLibrary.Extensions;

namespace ToolsLibrary.Converters
{
    public class ImageOrVideoCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.IsObjectNull())
            {
                if (parameter.IsObjectNull())
                {
                    return false;
                }

                if (value.ToString() == parameter.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
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