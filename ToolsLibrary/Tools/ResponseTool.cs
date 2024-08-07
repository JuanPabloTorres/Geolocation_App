namespace ToolsLibrary.Tools
{
    public class ResponseTool<T>
    {
        public T? Data { get; set; }

        public string Message { get; set; }

        public Type ResponseType { get; set; }

        public bool IsSuccess { get; set; }


    }

    public enum Type
    {
        Found,
        NotFound,
        Exist,
        NotExist,
        Exception,
        Updated,
        Added,
        Delete,
        Default,
        Fail,
        DataFound,
        EntityNotFound,
        Succesfully,
        IsRecoveryPassword,
        NotRecoveryPassword


    }
}
