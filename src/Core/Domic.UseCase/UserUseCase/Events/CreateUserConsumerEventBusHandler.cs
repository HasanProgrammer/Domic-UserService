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

    public Task BeforeHandleAsync(UserCreated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Users)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(UserCreated @event, CancellationToken cancellationToken)
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

        await _userQueryRepository.AddAsync(newUser, cancellationToken);
        
        var roleUsers = @event.Roles.Select(role => new RoleUserQuery {
            Id          = _globalUniqueIdGenerator.GetRandom(),
            UserId      = @event.Id,
            RoleId      = role,
            CreatedBy   = @event.CreatedBy,
            CreatedRole = @event.CreatedRole,
            CreatedAt_EnglishDate = @event.CreatedAt_EnglishDate,
            CreatedAt_PersianDate = @event.CreatedAt_PersianDate
        });
        
        var permissionUsers = @event.Permissions.Select(permission => new PermissionUserQuery {
            Id           = _globalUniqueIdGenerator.GetRandom(),
            UserId       = @event.Id,
            PermissionId = permission,
            CreatedBy    = @event.CreatedBy,
            CreatedRole  = @event.CreatedRole,
            CreatedAt_EnglishDate = @event.CreatedAt_EnglishDate,
            CreatedAt_PersianDate = @event.CreatedAt_PersianDate
        });
        
        await _roleUserQueryRepository.AddRangeAsync(roleUsers, cancellationToken);
        await _permissionUserQueryRepository.AddRangeAsync(permissionUsers, cancellationToken);
    }

    public Task AfterHandleAsync(UserCreated @event, CancellationToken cancellationToken) => Task.CompletedTask;
}