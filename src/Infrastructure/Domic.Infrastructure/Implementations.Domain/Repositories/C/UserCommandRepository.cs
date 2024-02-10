using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class UserCommandRepository : IUserCommandRepository
{
    private readonly SQLContext _context;

    public UserCommandRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class UserCommandRepository
{
    public async Task AddAsync(User entity, CancellationToken cancellationToken) 
        => await _context.Users.AddAsync(entity, cancellationToken);

    public void Change(User entity) => _context.Users.Update(entity);
}

//Query
public partial class UserCommandRepository
{
    public async Task<User> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _context.Users.Include(User => User.RoleUsers)
                               .ThenInclude(RoleUser => RoleUser.Role)
                               .Include(User => User.PermissionUsers)
                               .ThenInclude(PermissionUser => PermissionUser.Permission)
                               .ThenInclude(Permission => Permission.Role)
                               .FirstOrDefaultAsync(User => User.Id.Equals(id), cancellationToken);
    
    public async Task<User> FindByIdAsync(object id, CancellationToken cancellationToken) 
        => await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(id), cancellationToken);

    public async Task<User> FindByUsernameAsync(string username, CancellationToken cancellationToken) 
        => await _context.Users.FirstOrDefaultAsync(User => User.Username.Value.Equals(username), cancellationToken);

    public async Task<User> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken) 
        => await _context.Users.FirstOrDefaultAsync(User => User.PhoneNumber.Value.Equals(phoneNumber), cancellationToken);

    public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context.Users.FirstOrDefaultAsync(User => User.Email.Value.Equals(email), cancellationToken);

    public async Task<IEnumerable<User>> SearchEagerLoadingAsync(string username, string firstName, string lastName, 
        string phoneNumber, string email, string description, CancellationToken cancellationToken
    )
    {
        return await Task.Run(() => {
            
            IQueryable<User> Query = _context.Users.AsNoTracking()
                                                   .Include(User => User.RoleUsers)
                                                   .ThenInclude(RoleUser => RoleUser.Role)
                                                   .Include(User => User.PermissionUsers)
                                                   .ThenInclude(PermissionUser => PermissionUser.Permission)
                                                   .ThenInclude(Permission => Permission.Role);

            if (!string.IsNullOrEmpty(username))
                Query = Query.Where(User => EF.Functions.Like(User.Username.Value, $"%{username}%"));
            
            if(!string.IsNullOrEmpty(firstName))
                Query = Query.Where(User => EF.Functions.Like(User.FirstName.Value , $"%{firstName}%"));
            
            if(!string.IsNullOrEmpty(lastName))
                Query = Query.Where(User => EF.Functions.Like(User.LastName.Value , $"%{lastName}%"));
            
            if(!string.IsNullOrEmpty(phoneNumber))
                Query = Query.Where(User => EF.Functions.Like(User.PhoneNumber.Value , $"%{phoneNumber}%"));
            
            if(!string.IsNullOrEmpty(email))
                Query = Query.Where(User => EF.Functions.Like(User.Email.Value , $"%{email}%"));
            
            if(!string.IsNullOrEmpty(description))
                Query = Query.Where(User => EF.Functions.Like(User.Description.Value , $"%{description}%"));

            return Query;
            
        }, cancellationToken);
    }
}