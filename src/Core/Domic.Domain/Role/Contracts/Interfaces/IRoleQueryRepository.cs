using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Domain.Role.Entities;

namespace Domic.Domain.Role.Contracts.Interfaces;

public interface IRoleQueryRepository : IQueryRepository<RoleQuery, string>
{
    
}