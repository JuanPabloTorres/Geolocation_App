using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ToolsLibrary.Tools
{
    public class GenericTool<T>
    {
        public static void SetPropertyValueOnObject<TValue>(T obj, string propertyName, TValue value)
        {
            var property = typeof(T).GetProperty(propertyName);

            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, value);
            }
        }

        public static IEnumerable<PropertyInfo> GetPropertiesOfType(T obj)
        {
            return obj.GetType().GetProperties();
        }

        public static IEnumerable<object> GetSubPropertiesOfWithForeignKeyAttribute(T obj)
        {
            IList<object> propertiesIntance = new List<object>();

            var properties = GetPropertiesOfType(obj);

            foreach (PropertyInfo property in properties)
            {
                var foreignKeyAttribute = property.GetCustomAttribute<ForeignKeyAttribute>();

                if (foreignKeyAttribute != null)
                {
                    propertiesIntance.Add(property.GetValue(obj) as object);
                }
            }

            return propertiesIntance;
        }
    }
}