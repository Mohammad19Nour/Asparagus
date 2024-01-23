using AsparagusN.Entities;
using AsparagusN.Specifications;

namespace AsparagusN.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}