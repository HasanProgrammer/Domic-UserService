using Domic.Core.Domain.Enumerations;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class UserQueryRepository : IUserQueryRepository
{
    private readonly SQLContext _sqlContext;

    public UserQueryRepository(SQLContext sqlContext) => _sqlContext = sqlContext;
}

//Transaction
public partial class UserQueryRepository
{
    public void Add(UserQuery entity) => _sqlContext.Users.Add(entity);

    public Task AddAsync(UserQuery entity, CancellationToken cancellationToken)
    {
        _sqlContext.Users.Add(entity);

        return Task.CompletedTask;
    }

    public void Change(UserQuery entity) => _sqlContext.Users.Update(entity);

    public Task ChangeAsync(UserQuery entity, CancellationToken cancellationToken)
    {
        _sqlContext.Users.Update(entity);
        
        return Task.CompletedTask;
    }
}

//Query
public partial class UserQueryRepository
{
    public UserQuery FindById(object id) => _sqlContext.Users.FirstOrDefault(user => user.Id.Equals(id));

    public async Task<UserQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _sqlContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(id), cancellationToken);

    public async Task<UserQuery> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _sqlContext.Users.Include(User => User.RoleUsers)
                                  .ThenInclude(RoleUser => RoleUser.Role)
                                  .Include(User => User.PermissionUsers)
                                  .ThenInclude(PermissionUser => PermissionUser.Permission)
                                  .ThenInclude(Permission => Permission.Role)
                                  .FirstOrDefaultAsync(User => User.Id.Equals(id), cancellationToken);

    public async Task<IEnumerable<UserQuery>> FindAllWithOrderingAsync(Order order, bool accending, 
        CancellationToken cancellationToken
    )
    {
        var query = _sqlContext.Users.AsNoTracking();

        if (accending)
        {
            return order switch 
            {
                Order.Id   => await query.OrderBy(user => user.Id).ToListAsync(cancellationToken),
                Order.Date => await query.OrderBy(user => user.CreatedAt_EnglishDate).ToListAsync(cancellationToken),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        return order switch 
        {
            Order.Id   => await query.OrderByDescending(user => user.Id).ToListAsync(cancellationToken),
            Order.Date => await query.OrderByDescending(user => user.CreatedAt_EnglishDate).ToListAsync(cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}