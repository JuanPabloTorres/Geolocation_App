using GeolocationAdsAPI.Context;
using GeolocationAdsAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
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

            return ResponseFactory<T>.BuildSusccess("Created successfully.", entity);
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

            return ResponseFactory<T>.BuildSusccess("Created successfully.", entity);
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
                return ResponseFactory<T>.BuildSusccess("Found.", entity);
            }

            return ResponseFactory<T>.BuildFail("Not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
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
                return ResponseFactory<T>.BuildSusccess("Found.", entity, ToolsLibrary.Tools.Type.Found);
            }

            return ResponseFactory<T>.BuildFail("Not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
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

            return ResponseFactory<IEnumerable<T>>.BuildSusccess("Data Found successfully.", allEntities);
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

                return ResponseFactory<T>.BuildSusccess("Removed successfully.", entity);
            }

            return ResponseFactory<T>.BuildFail("Not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
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

                //_context.Entry(existingEntity).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return ResponseFactory<T>.BuildSusccess("Updated successfully.", entity);
            }

            return ResponseFactory<T>.BuildFail("Not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
        }
        catch (Exception ex)
        {
            return ResponseFactory<T>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
        }
    }

    public async Task<ResponseTool<T>> UpdateAsync(int id, T entity, params Expression<Func<T, object>>[] relatedExpressions)
    {
        try
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity != null)
            {
                foreach (var navigationProperty in relatedExpressions)
                {
                    var propertyName = GetPropertyName(navigationProperty);

                    var entry = _context.Entry(entity);

                    //if (entry.Reference(propertyName).IsLoaded)
                    //{
                    //    entry.Reference(propertyName).IsModified = true;
                    //}
                    if (entry.Collection(propertyName).IsLoaded)
                    {
                        entry.Collection(propertyName).IsModified = true;
                    }
                }

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

    //public async Task<ResponseTool<T>> UpdateAsync(int id, T entity, params Expression<Func<T, IEnumerable<object>>>[] relatedExpressions)
    //{
    //    try
    //    {
    //        var existingEntity = await _context.Set<T>().FindAsync(id);

    //        if (existingEntity != null)
    //        {
    //            foreach (var relatedExpression in relatedExpressions)
    //            {
    //                _context.Entry(existingEntity).Collection(relatedExpression).Query().Load();
    //            }

    //            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

    //            await _context.SaveChangesAsync();

    //            return ResponseFactory<T>.BuildSusccess("Entity updated successfully.", existingEntity);
    //        }

    //        return ResponseFactory<T>.BuildFail("Entity not found.", null, ToolsLibrary.Tools.Type.EntityNotFound);
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResponseFactory<T>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
    //    }
    //}

    // Helper method to dynamically create the predicate expression for the ID
    private Expression<Func<T, bool>> GetIdPredicate(int id)
    {
        var parameter = Expression.Parameter(typeof(T), "e");

        var property = Expression.Property(parameter, "ID");

        var idValue = Expression.Constant(id);

        var equalsExpression = Expression.Equal(property, idValue);

        return Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);
    }

    private string GetPropertyName(Expression<Func<T, object>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        else if (propertyExpression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand)
        {
            return operand.Member.Name;
        }
        throw new ArgumentException("Invalid property expression", nameof(propertyExpression));
    }

    private void UpdateRelatedData(T existingEntity, T updatedEntity)
    {
        // Check if the entity type has any navigation properties representing related data.
        var navigationProperties = _context.Entry(existingEntity).Navigations.ToList();

        foreach (var navigationProperty in navigationProperties)
        {
            // Use reflection to get the related data collection from the updated entity.
            var relatedDataCollection = updatedEntity.GetType().GetProperty(navigationProperty.Metadata.Name).GetValue(updatedEntity);

            // If it's a collection property, update the related data.
            if (navigationProperty is CollectionEntry collectionEntry)
            {
                // Get the existing related data collection.
                var existingCollection = collectionEntry.CurrentValue as IList;

                if (existingCollection != null)
                {
                    // Clear the existing related data collection.
                    existingCollection.Clear();

                    // Add the related data from the updated entity to the existing entity.
                    foreach (var relatedData in (IEnumerable<object>)relatedDataCollection)
                    {
                        existingCollection.Add(relatedData);
                    }
                }
            }
        }
    }
}