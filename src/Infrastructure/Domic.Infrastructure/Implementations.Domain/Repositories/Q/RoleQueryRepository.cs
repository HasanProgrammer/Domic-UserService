using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class RoleQueryRepository : IRoleQueryRepository
{
    private readonly SQLContext _context;

    public RoleQueryRepository(SQLContext context) => _context = context;
}

//Command
public partial class RoleQueryRepository
{
    public Task AddAsync(RoleQuery entity, CancellationToken cancellationToken)
    {
        _context.Roles.Add(entity);

        return Task.CompletedTask;
    }
    
    public Task ChangeAsync(RoleQuery entity, CancellationToken cancellationToken)
    {
        _context.Roles.Update(entity);

        return Task.CompletedTask;
    }
}

//Query
public partial class RoleQueryRepository
{
    public async ValueTask<long> CountRowsAsync(CancellationToken cancellationToken) 
        => await _context.Roles.CountAsync(cancellationToken);
    
    public async Task<RoleQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _context.Roles.FirstOrDefaultAsync(Role => Role.Id.Equals(id), cancellationToken);

    public async Task<RoleQuery> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _context.Roles.Where(Role => Role.Id.Equals(id))
                               .Include(Role => Role.Permissions)
                               .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<RoleQuery>> FindAllEagerLoadingAsync(CancellationToken cancellationToken) 
        => await _context.Roles.AsNoTracking()
                               .Include(Role => Role.Permissions)
                               .ToListAsync(cancellationToken);

    public async Task<IEnumerable<RoleQuery>> FindAllWithPaginateEagerLoadingAsync(int countPerPage, int pageNumber, 
        CancellationToken cancellationToken
    ) => await _context.Roles.AsNoTracking()
                             .Include(Role => Role.Permissions)
                             .Skip((pageNumber - 1)*countPerPage)
                             .Take(countPerPage)
                             .ToListAsync(cancellationToken);
}