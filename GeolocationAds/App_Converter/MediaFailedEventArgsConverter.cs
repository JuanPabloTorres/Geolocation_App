using CommunityToolkit.Maui.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationAds.App_Converter
{
    public  class MediaFailedEventArgsConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MediaFailedEventArgs mediaFailedChangedEventArgs)
            {
                return mediaFailedChangedEventArgs;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
