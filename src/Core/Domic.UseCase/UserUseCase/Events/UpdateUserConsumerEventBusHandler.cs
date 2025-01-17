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
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class UpdateUserConsumerEventBusHandler : IConsumerEventBusHandler<UserUpdated>
{
    private readonly IUserQueryRepository           _userQueryRepository;
    private readonly IRoleUserQueryRepository       _roleUserQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;
    private readonly IGlobalUniqueIdGenerator       _globalUniqueIdGenerator;

    public UpdateUserConsumerEventBusHandler(IUserQueryRepository userQueryRepository, 
        IRoleUserQueryRepository roleUserQueryRepository, IPermissionUserQueryRepository permissionUserQueryRepository, 
        IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _userQueryRepository           = userQueryRepository;
        _roleUserQueryRepository       = roleUserQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
        _globalUniqueIdGenerator       = globalUniqueIdGenerator;
    }

    public Task BeforeHandleAsync(UserUpdated @event, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Users)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(UserUpdated @event, CancellationToken cancellationToken)
    {
        var targetUser = await _userQueryRepository.FindByIdEagerLoadingAsync(@event.Id, cancellationToken);

        var roleUsers = @event.Roles.Select(role => new RoleUserQuery {
            Id          = _globalUniqueIdGenerator.GetRandom(),
            UserId      = @event.Id,
            RoleId      = role,
            CreatedBy   = @event.UpdatedBy,
            CreatedRole = @event.UpdatedRole,
            CreatedAt_EnglishDate = @event.UpdatedAt_EnglishDate,
            CreatedAt_PersianDate = @event.UpdatedAt_PersianDate
        });
        
        var permissionUsers = @event.Permissions.Select(permission => new PermissionUserQuery {
            Id           = _globalUniqueIdGenerator.GetRandom(),
            UserId       = @event.Id,
            PermissionId = permission,
            CreatedBy   = @event.UpdatedBy,
            CreatedRole = @event.UpdatedRole,
            CreatedAt_EnglishDate = @event.UpdatedAt_EnglishDate,
            CreatedAt_PersianDate = @event.UpdatedAt_PersianDate
        });
        
        targetUser.FirstName             = @event.FirstName;
        targetUser.LastName              = @event.LastName;
        targetUser.Username              = @event.Username;
        targetUser.Password              = @event.Password;
        targetUser.Description           = @event.Description;
        targetUser.PhoneNumber           = @event.PhoneNumber;
        targetUser.Email                 = @event.Email;
        targetUser.IsActive              = @event.IsActive ? IsActive.Active : IsActive.InActive;
        targetUser.UpdatedBy             = @event.UpdatedBy;
        targetUser.UpdatedRole           = @event.UpdatedRole;
        targetUser.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
        targetUser.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
                    
        await _userQueryRepository.ChangeAsync(targetUser, cancellationToken);
        await _roleUserQueryRepository.RemoveRangeAsync(targetUser.RoleUsers, cancellationToken);
        await _permissionUserQueryRepository.RemoveRangeAsync(targetUser.PermissionUsers, cancellationToken);
        await _roleUserQueryRepository.AddRangeAsync(roleUsers, cancellationToken);
        await _permissionUserQueryRepository.AddRangeAsync(permissionUsers, cancellationToken);
    }

    public Task AfterHandleAsync(UserUpdated @event, CancellationToken cancellationToken) => Task.CompletedTask;
}