using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public interface IBaseService<T> where T : class
    {
        Task<ResponseTool<T>> Add(T data);

        Task<ResponseTool<T>> Get(int Id);

        Task<ResponseTool<IEnumerable<T>>> GetAll();

        Task<ResponseTool<T>> Remove(int Id);

        Task<ResponseTool<T>> Update(T data, int currentId);
    }
}