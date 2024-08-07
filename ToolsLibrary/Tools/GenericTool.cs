using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using ToolsLibrary.Extensions;

namespace ToolsLibrary.Tools
{
    public class GenericTool<T>
    {
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

        public static void SetPropertyValueOnObject<TValue>(T obj, string propertyName, TValue value)
        {
            var property = typeof(T).GetProperty(propertyName);

            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, value);
            }
        }

        public static PValue GetPropertyValueFromObject<PValue>(object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);

            if (property != null && property.CanRead)
            {
                return (PValue)property.GetValue(obj);
            }

            return default(PValue);
        }

        public static async Task<RValue> InvokeMethodName<RValue>(object service, string methodName, object[] parameters)
        {
            try
            {
                MethodInfo _method = service.GetType().GetMethod(methodName);

                if (!_method.IsObjectNull())
                {
                    return await (Task<RValue>)_method.Invoke(service, parameters);
                }
                else
                {
                    return default(RValue);
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

                return default(RValue); ;
            }
        }
    }
}