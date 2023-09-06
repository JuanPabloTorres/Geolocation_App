using GeolocationAdsAPI.Context;
using GeolocationAdsAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

    public async Task<ResponseTool<T>> CreateAsync(T entity, params object[] relatedEntities)
    {
        try
        {
            _context.Set<T>().Add(entity);

            // Attach related entities if provided
            foreach (var relatedEntity in relatedEntities)
            {
                _context.Attach(relatedEntity);
            }

            await _context.SaveChangesAsync();

            return ResponseFactory<T>.BuildSusccess("Entity created successfully.", entity);
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

    public async Task<ResponseTool<T>> Get(int id, params Expression<Func<T, object>>[] includes)
    {
        try
        {
            var query = _context.Set<T>().AsQueryable();

            // Include navigation properties
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Create a predicate expression for the Where clause
            Expression<Func<T, bool>> predicate = GetIdPredicate(id);

            var entity = await query.FirstOrDefaultAsync(predicate);

            if (entity != null)
            {
                return ResponseFactory<T>.BuildSusccess("Entity found.", entity, ToolsLibrary.Tools.Type.Found);
            }

            return ResponseFactory<T>.BuildFail("Entity not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
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

    public async Task<ResponseTool<IEnumerable<T>>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {
        try
        {
            var query = _context.Set<T>().AsQueryable();

            // Include navigation properties
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Create a predicate expression for the Where clause
            //Expression<Func<T, bool>> predicate = GetIdPredicate(id);

            var entity = await query.ToListAsync();

            if (entity != null)
            {
                return ResponseFactory<IEnumerable<T>>.BuildSusccess("Entities found.", entity, ToolsLibrary.Tools.Type.Found);
            }

            return ResponseFactory<IEnumerable<T>>.BuildFail("Entities not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
        }
        catch (Exception ex)
        {
            return ResponseFactory<IEnumerable<T>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        }
    }

    public async Task<ResponseTool<T>> Remove(int id)
    {
        try
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);

                await _context.SaveChangesAsync();

                return ResponseFactory<T>.BuildSusccess("Entity removed successfully.", entity);
            }

            return ResponseFactory<T>.BuildFail("Entity not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
        }
        catch (Exception ex)
        {
            return ResponseFactory<T>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
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

    // Helper method to dynamically create the predicate expression for the ID
    private Expression<Func<T, bool>> GetIdPredicate(int id)
    {
        var parameter = Expression.Parameter(typeof(T), "e");

        var property = Expression.Property(parameter, "ID");

        var idValue = Expression.Constant(id);

        var equalsExpression = Expression.Equal(property, idValue);

        return Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);
    }
}