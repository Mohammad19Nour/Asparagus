using System.Collections;
using AsparagusN.Interfaces;

namespace AsparagusN.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private Hashtable? _repositories;    

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }


    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        _repositories ??= new Hashtable();
        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type)) return (IGenericRepository<TEntity>)_repositories[type]!;
        
        var repositoryType = typeof(GenericRepository<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType
            .MakeGenericType(typeof(TEntity)),_context);
        _repositories.Add(type,repositoryInstance);

        return (IGenericRepository<TEntity>)_repositories[type]!;
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
