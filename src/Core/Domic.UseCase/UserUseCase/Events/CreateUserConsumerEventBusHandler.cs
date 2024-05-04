using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.Extensions;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

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
    
    [WithCleanCache(Keies = Cache.Users)]
    [TransactionConfig(Type = TransactionType.Query)]
    public void Handle(UserCreated @event)
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

    public void AfterMaxRetryHandle(UserCreated @event){}

    /*---------------------------------------------------------------*/
    
    private void _roleUserBuilder(string createdBy, string createdRole, DateTime englishCreatedAt, 
        string persianCreatedAt, string userId, IEnumerable<string> roleIds
    )
    {
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