namespace ToolsLibrary.Factories
{
    public static class ModelFactory<T>
    {
        public static T Build(T model)
        {
            try
            {
                var _newModel = Activator.CreateInstance<T>();

                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    if (!property.CanWrite) // ❌ Evitar propiedades sin 'set'
                        continue;

                    var value = property.GetValue(model);

                    property.SetValue(_newModel, value);
                }

                return _newModel;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}