using Karami.Core.Domain.Contracts.Interfaces;

namespace Karami.Domain.Role.Contracts.Interfaces;

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
}