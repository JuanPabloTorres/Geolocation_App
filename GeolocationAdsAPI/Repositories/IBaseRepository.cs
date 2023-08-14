using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<ResponseTool<T>> CreateAsync(T entity);

        Task<ResponseTool<IEnumerable<T>>> GetAllAsync();

        Task<ResponseTool<T>> UpdateAsync(int id, T entity);

        Task<ResponseTool<T>> Get(int id);
    }
}