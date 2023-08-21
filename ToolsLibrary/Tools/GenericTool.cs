namespace ToolsLibrary.Tools
{
    public class GenericTool<T>
    {

        public static void SetPropertyOnObject<TValue>(T obj, string propertyName, TValue value)
        {
            var property = typeof(T).GetProperty(propertyName);

            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, value);
            }
        }

    }
}
