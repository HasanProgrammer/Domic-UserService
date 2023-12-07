using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Entities;
using Karami.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class PermissionUserCommandRepository : IPermissionUserCommandRepository
{
    private readonly SQLContext _context;

    public PermissionUserCommandRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class PermissionUserCommandRepository
{
    public async Task AddAsync(PermissionUser entity, CancellationToken cancellationToken) 
        => await _context.PermissionUsers.AddAsync(entity, cancellationToken);

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