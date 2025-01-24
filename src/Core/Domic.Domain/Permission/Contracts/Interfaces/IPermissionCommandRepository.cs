using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.Permission.Contracts.Interfaces;

public interface IPermissionCommandRepository : ICommandRepository<Entities.Permission, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> IsExistByNameAsync(string name, CancellationToken cancellationToken);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<Entities.Permission>> FindByRoleIdAsync(string roleId, CancellationToken cancellationToken) 
        => throw new NotImplementedException();
}