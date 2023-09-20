using ToolsLibrary.Tools;
using Type = ToolsLibrary.Tools.Type;

namespace ToolsLibrary.Factories
{
    public static class ResponseFactory<T>
    {
        public static ResponseTool<T> BuildSusccess(string message, T? data, Type type = Type.Default)
        {
            ResponseTool<T> __response = new ResponseTool<T>()
            {
                Data = data,
                Message = message,
                IsSuccess = true,
                ResponseType = type
            };

            return __response;
        }

        public static ResponseTool<T> BuildFail(string message, T? data, Type type = Type.Default)
        {
            ResponseTool<T> __response = new ResponseTool<T>()
            {
                Data = data,
                Message = message,
                IsSuccess = false,
                ResponseType = type
            };

            return __response;
        }
    }
}