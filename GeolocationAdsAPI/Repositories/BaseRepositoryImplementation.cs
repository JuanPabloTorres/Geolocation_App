using GeolocationAdsAPI.Context;
using GeolocationAdsAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using ToolsLibrary.Factories;
using ToolsLibrary.Tools;

public class BaseRepositoryImplementation<T> : IBaseRepository<T> where T : class
{
    protected readonly GeolocationContext _context;

    public BaseRepositoryImplementation(GeolocationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ResponseTool<T>> CreateAsync(T entity)
    {
        try
        {
            _context.Set<T>().Add(entity);

            await _context.SaveChangesAsync();

            return ResponseFactory<T>.BuildSusccess("Entity created successfully.", entity);

        }
        catch (Exception ex)
        {
            return ResponseFactory<T>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        }
    }

    public async Task<ResponseTool<IEnumerable<T>>> GetAllAsync()
    {
        try
        {
            var allEntities = await _context.Set<T>().ToListAsync();

            return ResponseFactory<IEnumerable<T>>.BuildSusccess("Entities fetched successfully.", allEntities);
        }
        catch (Exception ex)
        {
            return ResponseFactory<IEnumerable<T>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        }
    }

    public async Task<ResponseTool<T>> UpdateAsync(int id, T entity)
    {
        try
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);

                await _context.SaveChangesAsync();

                return ResponseFactory<T>.BuildSusccess("Entity updated successfully.", existingEntity);
            }

            return ResponseFactory<T>.BuildFail("Entity not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
        }
        catch (Exception ex)
        {
            return ResponseFactory<T>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        }
    }

    public async Task<ResponseTool<T>> Get(int id)
    {
        try
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                return ResponseFactory<T>.BuildSusccess("Entity found.", entity);
            }

            return ResponseFactory<T>.BuildFail("Entity not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
        }
        catch (Exception ex)
        {
            return ResponseFactory<T>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        }
    }
}
