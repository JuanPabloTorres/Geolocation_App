using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Enums;

namespace ToolsLibrary.Converters
{
    public class UserProviderToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Providers provider)
            {
                return provider == Providers.App; // Solo habilita si el usuario es interno (App)
            }

            return false; // Deshabilitado en caso de Google o Facebook
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // No se necesita convertir de vuelta
        }
    }
}
