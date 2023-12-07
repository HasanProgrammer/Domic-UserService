using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Domain.Permission.Entities;

namespace Karami.Domain.Permission.Contracts.Interfaces;

public interface IPermissionQueryRepository : IQueryRepository<PermissionQuery, string>
{
    
}