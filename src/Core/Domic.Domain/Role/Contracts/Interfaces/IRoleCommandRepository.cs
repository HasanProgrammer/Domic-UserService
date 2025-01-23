using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.Role.Contracts.Interfaces;

public interface IRoleCommandRepository : ICommandRepository<Entities.Role, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<Entities.Role> FindByNameAsync(string name, CancellationToken cancellationToken) 
        => throw new NotImplementedException();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> IsExistByIdAsync(string id, CancellationToken cancellationToken);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> IsExistByNameAsync(string name, CancellationToken cancellationToken);
}