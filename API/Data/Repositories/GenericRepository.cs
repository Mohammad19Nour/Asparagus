using AsparagusN.Entities;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Data;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;

    public GenericRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    
    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec)
    {
        return (await ApplySpecification(spec).ToListAsync())!;
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }


    public IQueryable<T> GetQueryable()
    {
        return _context.Set<T>().AsQueryable();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public void SoftDelete(T entity)
    {
        var softDeleteInterface = typeof(ISoftDeletable);
        if (softDeleteInterface.IsAssignableFrom(typeof(T)))
        {
            var softDeleteProperty = typeof(T).GetProperty("IsDeleted");
            
            if (softDeleteProperty == null || softDeleteProperty.PropertyType != typeof(bool))
                throw new InvalidOperationException("The entity does not support soft delete.");
         
            softDeleteProperty.SetValue(entity, true);
            _context.Entry(entity).State = EntityState.Modified;
            return;
        }

        throw new InvalidOperationException("The entity does not support soft delete.");
    }

    private IQueryable<T?> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(),spec);
    }
    private bool IsSoftDeleted(T entity)
    {
        var softDeleteInterface = typeof(ISoftDeletable);
        if (softDeleteInterface.IsAssignableFrom(typeof(T)))
        {
            var softDeleteProperty = typeof(T).GetProperty("IsDeleted");
            if (softDeleteProperty != null && softDeleteProperty.PropertyType == typeof(bool))
            {
                var value = (bool)softDeleteProperty.GetValue(entity);
                return value;
            }
        }
        return false;
    }
}