using Karami.Core.Domain.Contracts.Interfaces;

namespace Karami.Domain.PermissionUser.Contracts.Interfaces;

public interface IPermissionUserCommandRepository : ICommandRepository<Entities.PermissionUser, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<Entities.PermissionUser>> FindAllByUserIdAsync(string userId, 
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<Entities.PermissionUser>> FindAllByPermissionIdAsync(string permissionId, 
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();
}