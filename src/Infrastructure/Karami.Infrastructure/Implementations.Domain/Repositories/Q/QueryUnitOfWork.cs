using System.Data;
using Karami.Domain.Commons.Contracts.Interfaces;
using Karami.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.Q;

public class QueryUnitOfWork : IQueryUnitOfWork
{
    private readonly SQLContext   _context;
    private IDbContextTransaction _transaction;
    
    public QueryUnitOfWork(SQLContext context) => _context = context; //Resource

    public void Transaction(IsolationLevel isolationLevel) 
        => _transaction = _context.Database.BeginTransaction(isolationLevel); //Resource

    public void Commit()
    {
        _context.SaveChanges();
        _transaction.Commit();
    }

    public void Rollback() => _transaction?.Rollback();

    public void Dispose() => _transaction?.Dispose();
}