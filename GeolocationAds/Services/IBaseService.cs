using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IBaseService<T> where T : class
    {
        Task<ResponseTool<IEnumerable<T>>> GetAll();

        Task<ResponseTool<T>> Get(Guid Id);

        Task<ResponseTool<T>> Remove(Guid Id);

        Task<ResponseTool<T>> Add(T data);

        Task<ResponseTool<T>> Update(T data, Guid currentId);




    }
}
