using Karami.Domain.Commons.Contracts.Interfaces;
using Karami.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.C;

public class CommandUnitOfWork : ICommandUnitOfWork
{
    private readonly SQLContext   _context;
    private IDbContextTransaction _transaction;

    public CommandUnitOfWork(SQLContext context) => _context = context; //Resource

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