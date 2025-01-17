using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class PermissionUserQueryRepository : IPermissionUserQueryRepository
{
    private readonly SQLContext _sqlContext;

    public PermissionUserQueryRepository(SQLContext sqlContext) => _sqlContext = sqlContext;
}

//Transaction
public partial class PermissionUserQueryRepository
{
    public Task AddRangeAsync(IEnumerable<PermissionUserQuery> entities, CancellationToken cancellationToken)
    {
        _sqlContext.PermissionUsers.AddRange(entities);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<PermissionUserQuery> entities, CancellationToken cancellationToken)
    {
        _sqlContext.PermissionUsers.RemoveRange(entities);

        return Task.CompletedTask;
    }
}

//Query
public partial class PermissionUserQueryRepository
{
    public async Task<IEnumerable<PermissionUserQuery>> FindAllByUserIdAsync(string userId, 
        CancellationToken cancellationToken
    ) => await _sqlContext.PermissionUsers.Where(pu => pu.UserId.Equals(userId)).ToListAsync(cancellationToken);

    public async Task<IEnumerable<PermissionUserQuery>> FindAllByPermissionIdAsync(string permissionId, 
        CancellationToken cancellationToken
    ) => await _sqlContext.PermissionUsers.Where(pu => pu.PermissionId.Equals(permissionId))
                                          .ToListAsync(cancellationToken);
}