using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Domain.Role.Entities;

namespace Karami.Domain.Role.Contracts.Interfaces;

public interface IRoleQueryRepository : IQueryRepository<RoleQuery, string>
{
    
}