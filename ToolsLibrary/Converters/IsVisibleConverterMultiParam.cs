using System.Globalization;

namespace ToolsLibrary.Converters
{
    public class IsVisibleConverterMultiParam : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 1 && values[0] is bool isLoading && values[1] is int count)
            {
                // Si está cargando, retorna false para esconderlo.
                if (isLoading)
                {
                    return false;
                }

                // Si el count es mayor que cero, retorna true para mostrarlo.
                return count > 0;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
