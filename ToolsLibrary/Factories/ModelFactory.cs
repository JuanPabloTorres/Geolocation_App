namespace ToolsLibrary.Factories
{
    public static class ModelFactory<T>
    {
        public static T Build(T model)
        {
            var _newModel = Activator.CreateInstance<T>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(model);

                property.SetValue(_newModel, value);
            }

            return _newModel;
        }

        public static T Build(object model)
        {
            var _newModel = Activator.CreateInstance<T>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(model);

                property.SetValue(_newModel, value);
            }

            return _newModel;
        }
    }
}