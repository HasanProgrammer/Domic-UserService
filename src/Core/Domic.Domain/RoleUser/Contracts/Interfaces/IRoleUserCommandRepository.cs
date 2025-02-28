using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.RoleUser.Contracts.Interfaces;

public interface IRoleUserCommandRepository : ICommandRepository<Entities.RoleUser, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<Entities.RoleUser>> FindAllByUserIdAsync(string userId, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<Entities.RoleUser>> FindAllByRoleIdAsync(string roleId, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task AddRangeAsync(IEnumerable<Entities.RoleUser> entities, CancellationToken cancellationToken);
}