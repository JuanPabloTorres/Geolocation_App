using System.Globalization;

namespace GeolocationAds.App_Converter
{
    public class AnimationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isAnimating)
            {
                // Assuming the .gif is always running if attached and rendered
                // Simply return true to keep it running if IsAnimation is true
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TwoWay binding may require conversion back, but for IsAnimation it's typically not needed.
            // You can implement this if your application's logic requires it.
            return true;
        }
    }
}
