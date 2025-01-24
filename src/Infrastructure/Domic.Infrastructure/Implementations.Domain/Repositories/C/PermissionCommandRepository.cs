using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

public partial class PermissionCommandRepository : IPermissionCommandRepository
{
    private readonly SQLContext _Context;

    public PermissionCommandRepository(SQLContext Context) => _Context = Context;
}

public partial class PermissionCommandRepository
{
    public async Task AddAsync(Permission entity, CancellationToken cancellationToken) 
        => await _Context.Permissions.AddAsync(entity, cancellationToken);

    public Task ChangeAsync(Permission entity, CancellationToken cancellationToken)
    {
        _Context.Permissions.Update(entity);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<Permission> entities, CancellationToken cancellationToken)
    {
        _Context.Permissions.RemoveRange(entities);

        return Task.CompletedTask;
    }
}

public partial class PermissionCommandRepository
{
    public async ValueTask<long> CountRowsAsync(CancellationToken cancellationToken) 
        => await _Context.Permissions.CountAsync(cancellationToken);

    public async Task<IEnumerable<Permission>> FindAllWithPaginateEagerLoadingAsync(int countPerPage, int pageNumber, 
        CancellationToken cancellationToken
    ) => await _Context.Permissions.AsNoTracking()
                                   .Include(Permission => Permission.Role)
                                   .Skip((pageNumber - 1)*countPerPage)
                                   .Take(countPerPage)
                                   .ToListAsync(cancellationToken);

    public async Task<Permission> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _Context.Permissions.FirstOrDefaultAsync(Permission => Permission.Id.Equals(id), cancellationToken);

    public async Task<Permission> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _Context.Permissions.Where(Permission => Permission.Id.Equals(id))
                                     .Include(Permission => Permission.Role)
                                     .SingleOrDefaultAsync(cancellationToken);

    public Task<bool> IsExistByNameAsync(string name, CancellationToken cancellationToken)
        => _Context.Permissions.AnyAsync(permission => permission.Name.Value == name, cancellationToken);

    public async Task<IEnumerable<Permission>> FindByRoleIdAsync(string roleId, CancellationToken cancellationToken)
        => await _Context.Permissions.Where(Permission => Permission.RoleId.Equals(roleId))
                                     .ToListAsync(cancellationToken);
}