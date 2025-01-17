using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class RoleUserCommandRepository : IRoleUserCommandRepository
{
    private readonly SQLContext _context;

    public RoleUserCommandRepository(SQLContext context) => _context = context;
}

//Command
public partial class RoleUserCommandRepository
{
    public Task AddAsync(RoleUser entity, CancellationToken cancellationToken)
    {
        _context.RoleUsers.Add(entity);

        return Task.CompletedTask;
    }

    public Task AddRangeAsync(IEnumerable<RoleUser> entities, CancellationToken cancellationToken)
    {
        _context.RoleUsers.AddRange(entities);

        return Task.CompletedTask;
    }

    public void Remove(RoleUser entity) => _context.RoleUsers.Remove(entity);

    public void RemoveRange(IEnumerable<RoleUser> entities) => _context.RoleUsers.RemoveRange(entities);
}

//Query
public partial class RoleUserCommandRepository
{
    public async Task<IEnumerable<RoleUser>> FindAllByUserIdAsync(string userId, CancellationToken cancellationToken) 
        => await _context.RoleUsers.Where( Role => Role.UserId.Equals(userId) ).ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<RoleUser>> FindAllByRoleIdAsync(string roleId, CancellationToken cancellationToken) 
        => await _context.RoleUsers.Where( Role => Role.RoleId.Equals(roleId) ).ToListAsync(cancellationToken);
}