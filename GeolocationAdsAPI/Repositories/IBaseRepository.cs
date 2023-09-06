using System.Linq.Expressions;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<ResponseTool<T>> CreateAsync(T entity);

        Task<ResponseTool<T>> CreateAsync(T entity, params object[] relatedEntities);

        Task<ResponseTool<T>> Get(int id);

        Task<ResponseTool<T>> Get(int id, params Expression<Func<T, object>>[] includes);

        Task<ResponseTool<IEnumerable<T>>> GetAllAsync();

        Task<ResponseTool<IEnumerable<T>>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

        Task<ResponseTool<T>> Remove(int id);

        Task<ResponseTool<T>> UpdateAsync(int id, T entity);
    }
}