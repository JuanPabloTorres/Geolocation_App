using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Primitives;
using Microsoft.Maui.Controls;

namespace GeolocationAds.App_Converter
{
   
        public class MediaStateChangedEventArgsConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is MediaStateChangedEventArgs mediaStateChangedEventArgs)
                {
                    return mediaStateChangedEventArgs;
                }
                return null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    
}
