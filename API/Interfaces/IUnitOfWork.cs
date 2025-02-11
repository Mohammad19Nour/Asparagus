﻿using Microsoft.EntityFrameworkCore.Storage;

namespace AsparagusN.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<bool> SaveChanges();
    IDbContextTransaction BeginTransaction();
    bool HasChanges();
}