using System.Globalization;
using ToolsLibrary.Models;

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

        public static void SetUpdateInformation(this object obj, int updatedBy)
        {
            var _objectProperties = obj.GetType().GetProperties().Where(v => v.Name == nameof(BaseModel.UpdateBy) || v.Name == nameof(BaseModel.UpdateDate)).ToList();

            if (!_objectProperties.IsObjectNull())
            {
                foreach (var item in _objectProperties)
                {
                    if (item.Name == nameof(BaseModel.UpdateBy))
                    {
                        item.SetValue(obj, updatedBy);
                    }

                    if (item.Name == nameof(BaseModel.UpdateDate))
                    {
                        item.SetValue(obj, DateTime.Now);
                    }
                }
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