using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;

namespace Domic.Domain.Permission.Contracts.Interfaces;

public interface IPermissionQueryRepository : IQueryRepository<PermissionQuery, string>
{
    
}