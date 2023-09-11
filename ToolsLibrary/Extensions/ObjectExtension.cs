using System.Globalization;

namespace ToolsLibrary.Extensions
{
    public static class ObjectExtension
    {
        public static bool IsObjectNull(this object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNotNullOrCountGreaterZero(this IEnumerable<object> obj)
        {
            if (obj != null && obj.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ToCamelCase(this string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            input = input.ToLower(); // Convert the input to lowercase

            input = textInfo.ToTitleCase(input); // Convert to title case

            input = input.Replace(" ", ""); // Remove spaces

            return char.ToLower(input[0]) + input.Substring(1);
        }
    }
}