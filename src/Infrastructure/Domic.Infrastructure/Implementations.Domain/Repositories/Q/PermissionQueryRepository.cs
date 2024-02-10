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
    public void Add(PermissionQuery entity) => _context.Permissions.Add(entity);

    public void Change(PermissionQuery entity) => _context.Permissions.Update(entity);

    public void Remove(PermissionQuery entity) => _context.Permissions.Remove(entity);

    public void RemoveRange(IEnumerable<PermissionQuery> entities) => _context.Permissions.RemoveRange(entities);

    public void SoftDelete(PermissionQuery entity) => _context.Permissions.Update(entity);
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

    public async Task<PermissionQuery> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _context.Permissions.Where(Permission => Permission.Id.Equals(id))
                                     .Include(Permission => Permission.Role)
                                     .SingleOrDefaultAsync(cancellationToken);
}