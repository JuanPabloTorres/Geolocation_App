using System.Collections;
using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class SpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
            {
                //You can adjust the calculation as needed
                int desiredSpan = Math.Max(2, collection.Count / 2); // Set a minimum of 1

                //if (collection.Count > 1)
                //{
                //    return collection.Count;
                //}
                //else
                //{
                //    return 1;
                //}

                return desiredSpan;
            }

            return 1; // Default span value
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}