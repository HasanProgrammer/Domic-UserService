using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;

namespace Domic.Domain.Permission.Contracts.Interfaces;

public interface IPermissionQueryRepository : IQueryRepository<PermissionQuery, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public new Task<List<PermissionQuery>> FindAllEagerLoadingAsync(CancellationToken cancellationToken);
}