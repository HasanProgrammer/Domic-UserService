using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Domain.User.Entities;

namespace Domic.Domain.User.Contracts.Interfaces;

public interface IUserQueryRepository : IQueryRepository<UserQuery, string>;