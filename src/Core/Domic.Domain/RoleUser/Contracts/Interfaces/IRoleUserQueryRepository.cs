using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Domain.RoleUser.Entities;

namespace Domic.Domain.RoleUser.Contracts.Interfaces;

public interface IRoleUserQueryRepository : IQueryRepository<RoleUserQuery, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<RoleUserQuery>> FindAllByUserIdAsync(string userId, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<RoleUserQuery>> FindAllByRoleIdAsync(string roleId, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task AddRangeAsync(IEnumerable<RoleUserQuery> entities, CancellationToken cancellationToken);
}