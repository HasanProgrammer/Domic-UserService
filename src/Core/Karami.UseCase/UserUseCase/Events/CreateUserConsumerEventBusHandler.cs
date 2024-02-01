using System.Data;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.RoleUser.Contracts.Interfaces;
using Karami.Domain.RoleUser.Entities;
using Karami.Domain.User.Contracts.Interfaces;
using Karami.Domain.User.Entities;
using Karami.Domain.User.Events;

namespace Karami.UseCase.UserUseCase.Events;

public class CreateUserConsumerEventBusHandler : IConsumerEventBusHandler<UserCreated>
{
    private readonly IUserQueryRepository           _userQueryRepository;
    private readonly IRoleUserQueryRepository       _roleUserQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;
    private readonly IGlobalUniqueIdGenerator       _globalUniqueIdGenerator;

    public CreateUserConsumerEventBusHandler(IUserQueryRepository userQueryRepository, 
        IRoleUserQueryRepository roleUserQueryRepository, IPermissionUserQueryRepository permissionUserQueryRepository,
        IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _userQueryRepository           = userQueryRepository;
        _roleUserQueryRepository       = roleUserQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
        _globalUniqueIdGenerator       = globalUniqueIdGenerator;
    }
    
    [WithTransaction(IsolationLevel = IsolationLevel.ReadUncommitted)]
    [WithCleanCache(Keies = Cache.Users)]
    public void Handle(UserCreated @event)
    {
        var targetUser = _userQueryRepository.FindByIdAsync(@event.Id, default).Result;

        if (targetUser is null) //Replication management
        {
            var newUser = new UserQuery {
                Id                    = @event.Id                                             ,
                CreatedBy             = @event.CreatedBy                                      , 
                CreatedRole           = @event.CreatedRole                                    , 
                FirstName             = @event.FirstName                                      ,
                LastName              = @event.LastName                                       ,
                Username              = @event.Username                                       ,
                Description           = @event.Description                                    ,
                PhoneNumber           = @event.PhoneNumber                                    ,
                Email                 = @event.Email                                          ,
                Password              = @event.Password.HashAsync(default).Result             ,
                IsActive              = @event.IsActive ? IsActive.Active : IsActive.InActive ,
                CreatedAt_EnglishDate = @event.CreatedAt_EnglishDate                          ,
                CreatedAt_PersianDate = @event.CreatedAt_PersianDate
            };

            _userQueryRepository.Add(newUser);
        
            _roleUserBuilder(@event.CreatedBy, @event.CreatedRole, 
                @event.CreatedAt_EnglishDate, @event.CreatedAt_PersianDate, @event.Id, @event.Roles
            );
            
            _permissionUserBuilder(@event.CreatedBy, @event.CreatedRole, 
                @event.CreatedAt_EnglishDate, @event.CreatedAt_PersianDate, @event.Id, @event.Permissions
            );
        }
    }

    /*---------------------------------------------------------------*/
    
    private void _roleUserBuilder(string createdBy, string createdRole, DateTime englishCreatedAt, 
        string persianCreatedAt, string userId, IEnumerable<string> roleIds
    )
    {
        var roleUsers = _roleUserQueryRepository.FindAllByUserIdAsync(userId, default).GetAwaiter().GetResult();
        
        //1 . Remove already user roles
        _roleUserQueryRepository.RemoveRange(roleUsers);
        
        //2 . Assign new roles for user
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUserQuery {
                Id          = _globalUniqueIdGenerator.GetRandom(),
                CreatedBy   = createdBy,
                CreatedRole = createdRole,
                UserId      = userId,
                RoleId      = roleId,
                CreatedAt_EnglishDate = englishCreatedAt,
                CreatedAt_PersianDate = persianCreatedAt
            };

            _roleUserQueryRepository.Add(newRoleUser);
        }
    }
    
    private void _permissionUserBuilder(string createdBy, string createdRole, DateTime englishCreatedAt, 
        string persianCreatedAt, string userId, IEnumerable<string> permissionIds
    )
    {
        var permissionUsers =
            _permissionUserQueryRepository.FindAllByUserIdAsync(userId, default).GetAwaiter().GetResult();
        
        //1 . Remove already user permissions
        _permissionUserQueryRepository.RemoveRange(permissionUsers);
        
        //2 . Assign new permissions for user
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUserQuery {
                Id           = _globalUniqueIdGenerator.GetRandom(),
                CreatedBy    = createdBy,
                CreatedRole  = createdRole,
                UserId       = userId,
                PermissionId = permissionId,
                CreatedAt_EnglishDate = englishCreatedAt,
                CreatedAt_PersianDate = persianCreatedAt
            };

            _permissionUserQueryRepository.Add(newPermissionUser);
        }
    }
}