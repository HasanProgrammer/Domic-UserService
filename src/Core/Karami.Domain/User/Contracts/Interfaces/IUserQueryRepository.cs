using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Domain.User.Entities;

namespace Karami.Domain.User.Contracts.Interfaces;

public interface IUserQueryRepository : IQueryRepository<UserQuery, string>
{
    
}