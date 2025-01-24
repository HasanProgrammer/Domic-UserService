using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class PermissionQueryRepository : IPermissionQueryRepository
{
    private readonly SQLContext _context;

    public PermissionQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class PermissionQueryRepository
{
    public Task AddAsync(PermissionQuery entity, CancellationToken cancellationToken)
    {
        _context.Permissions.Add(entity);

        return Task.CompletedTask;
    }

    public Task ChangeAsync(PermissionQuery entity, CancellationToken cancellationToken)
    {
        _context.Permissions.Update(entity);

        return Task.CompletedTask;
    }
}

//Query
public partial class PermissionQueryRepository
{
    public async ValueTask<long> CountRowsAsync(CancellationToken cancellationToken) 
        => await _context.Permissions.CountAsync(cancellationToken);

    public async Task<IEnumerable<PermissionQuery>> FindAllWithPaginateEagerLoadingAsync(int countPerPage, int pageNumber, 
        CancellationToken cancellationToken
    ) => await _context.Permissions.AsNoTracking()
                                   .Include(Permission => Permission.Role)
                                   .Skip((pageNumber - 1)*countPerPage)
                                   .Take(countPerPage)
                                   .ToListAsync(cancellationToken);

    public async Task<PermissionQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _context.Permissions.FirstOrDefaultAsync(Permission => Permission.Id.Equals(id), cancellationToken);

    public async Task<IEnumerable<PermissionQuery>> FindAllAsync(CancellationToken cancellationToken)
        => await _context.Permissions.AsNoTracking().ToListAsync(cancellationToken);

    public Task<List<PermissionQuery>> FindAllEagerLoadingAsync(CancellationToken cancellationToken) 
        => _context.Permissions.AsNoTracking().Include(Permission => Permission.Role).ToListAsync(cancellationToken);

    public async Task<PermissionQuery> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _context.Permissions.Where(Permission => Permission.Id.Equals(id))
                                     .Include(Permission => Permission.Role)
                                     .SingleOrDefaultAsync(cancellationToken);
}