using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Entities;

namespace Karami.Domain.PermissionUser.Contracts.Interfaces;

public interface IPermissionUserQueryRepository : IQueryRepository<PermissionUserQuery, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<PermissionUserQuery>> FindAllByUserIdAsync(string userId, 
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<PermissionUserQuery>> FindAllByPermissionIdAsync(string permissionId, 
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();
}