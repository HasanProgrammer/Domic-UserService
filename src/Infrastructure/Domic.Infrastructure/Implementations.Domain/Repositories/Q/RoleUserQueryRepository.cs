using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class RoleUserQueryRepository : IRoleUserQueryRepository
{
    private readonly SQLContext _sqlContext;

    public RoleUserQueryRepository(SQLContext sqlContext) => _sqlContext = sqlContext;
}

//Transaction
public partial class RoleUserQueryRepository
{
    public void Add(RoleUserQuery entity) => _sqlContext.RoleUsers.Add(entity);

    public void Remove(RoleUserQuery entity) => _sqlContext.RoleUsers.Remove(entity);

    public void RemoveRange(IEnumerable<RoleUserQuery> entities) => _sqlContext.RoleUsers.RemoveRange(entities);
}

//Query
public partial class RoleUserQueryRepository
{
    public async Task<IEnumerable<RoleUserQuery>> FindAllByUserIdAsync(string userId,
        CancellationToken cancellationToken
    ) => await _sqlContext.RoleUsers.Where(roleUser => roleUser.UserId.Equals(userId)).ToListAsync(cancellationToken);

    public async Task<IEnumerable<RoleUserQuery>> FindAllByRoleIdAsync(string roleId, 
        CancellationToken cancellationToken
    ) => await _sqlContext.RoleUsers.Where(roleUser => roleUser.RoleId.Equals(roleId)).ToListAsync(cancellationToken);
}