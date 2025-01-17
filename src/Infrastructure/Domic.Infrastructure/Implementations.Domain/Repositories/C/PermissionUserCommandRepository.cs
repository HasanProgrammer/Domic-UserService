using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class PermissionUserCommandRepository : IPermissionUserCommandRepository
{
    private readonly SQLContext _context;

    public PermissionUserCommandRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class PermissionUserCommandRepository
{
    public Task AddAsync(PermissionUser entity, CancellationToken cancellationToken)
    {
        _context.PermissionUsers.Add(entity);

        return Task.CompletedTask;
    }

    public Task AddRangeAsync(IEnumerable<PermissionUser> entities, CancellationToken cancellationToken)
    {
        _context.PermissionUsers.AddRange(entities);

        return Task.CompletedTask;
    }

    public void Remove(PermissionUser entity) => _context.PermissionUsers.Remove(entity);

    public void RemoveRange(IEnumerable<PermissionUser> entities) => _context.PermissionUsers.RemoveRange(entities);
}

//Query
public partial class PermissionUserCommandRepository
{
    public async Task<IEnumerable<PermissionUser>> FindAllByUserIdAsync(string userId, 
        CancellationToken cancellationToken
    ) => await _context.PermissionUsers.Where(pu => pu.UserId.Equals(userId)).ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<PermissionUser>> FindAllByPermissionIdAsync(string permissionId, 
        CancellationToken cancellationToken
    ) => await _context.PermissionUsers.Where(pu => pu.PermissionId.Equals(permissionId)).ToListAsync(cancellationToken);
}