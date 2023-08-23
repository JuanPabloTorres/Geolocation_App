using ToolsLibrary.Models;

namespace ToolsLibrary.Tools
{
    public class LogUserPerfilTool
    {
        public User LogUser { get; set; }

        public string JsonToken { get; set; }

        public T GetLogUserPropertyValue<T>(string propertyName)
        {
            var _value = GenericTool<T>.GetPropertyValueFromObject<T>(this.LogUser, propertyName);

            return _value;
        }

    }
}
