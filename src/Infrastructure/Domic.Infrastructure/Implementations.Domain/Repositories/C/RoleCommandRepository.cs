using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class RoleCommandRepository : IRoleCommandRepository
{
    private readonly SQLContext _context;

    public RoleCommandRepository(SQLContext context) => _context = context;
}

//Command
public partial class RoleCommandRepository
{
    public async Task AddAsync(Role entity, CancellationToken cancellationToken) 
        => await _context.Roles.AddAsync(entity, cancellationToken);

    public Task ChangeAsync(Role entity, CancellationToken cancellationToken)
    {
        _context.Roles.Update(entity);

        return Task.CompletedTask;
    }
}

//Query
public partial class RoleCommandRepository
{
    public async ValueTask<long> CountRowsAsync(CancellationToken cancellationToken) 
        => await _context.Roles.CountAsync(cancellationToken);

    public Task<bool> IsExistByIdAsync(string id, CancellationToken cancellationToken) 
        => _context.Roles.AnyAsync(r => r.Id == id, cancellationToken);
    
    public Task<bool> IsExistByNameAsync(string name, CancellationToken cancellationToken) 
        => _context.Roles.AnyAsync(r => r.Name.Value == name, cancellationToken);

    public async Task<Role> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _context.Roles.FirstOrDefaultAsync(Role => Role.Id.Equals(id), cancellationToken);

    public async Task<Role> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _context.Roles.Where(Role => Role.Id.Equals(id))
                               .Include(Role => Role.Permissions)
                               .FirstOrDefaultAsync(cancellationToken);

    public async Task<Role> FindByNameAsync(string name, CancellationToken cancellationToken)
        => await _context.Roles.FirstOrDefaultAsync(Role => Role.Name.Value.Equals(name), cancellationToken);

    public async Task<IEnumerable<Role>> FindAllWithPaginateEagerLoadingAsync(int countPerPage, int pageNumber, CancellationToken cancellationToken)
        => await _context.Roles.AsNoTracking()
                               .Include(Role => Role.Permissions)
                               .Skip((pageNumber - 1)*countPerPage)
                               .Take(countPerPage)
                               .ToListAsync(cancellationToken);
}