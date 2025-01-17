using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Events;
using Domic.Domain.RoleUser.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Events;

public class DeleteRoleConsumerEventBusHandler : IConsumerEventBusHandler<RoleDeleted>
{
    private readonly IRoleQueryRepository           _roleQueryRepository;
    private readonly IPermissionQueryRepository     _permissionQueryRepository;
    private readonly IRoleUserQueryRepository       _roleUserQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;

    public DeleteRoleConsumerEventBusHandler(IRoleQueryRepository roleQueryRepository,
        IPermissionQueryRepository permissionQueryRepository, IRoleUserQueryRepository roleUserQueryRepository,
        IPermissionUserQueryRepository permissionUserQueryRepository
    )
    {
        _roleQueryRepository           = roleQueryRepository;
        _permissionQueryRepository     = permissionQueryRepository;
        _roleUserQueryRepository       = roleUserQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
    }

    public Task BeforeHandleAsync(RoleDeleted @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Roles)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(RoleDeleted @event, CancellationToken cancellationToken)
    {
        var targetRole = await _roleQueryRepository.FindByIdEagerLoadingAsync(@event.Id, cancellationToken);

        if (targetRole is not null) //Replication management
        {
            #region SoftDelete Role

            targetRole.IsDeleted = IsDeleted.Delete;
            
            await _roleQueryRepository.ChangeAsync(targetRole, cancellationToken);

            #endregion
        
            #region SoftDelete Permission

            foreach (var permission in targetRole.Permissions)
            {
                permission.IsDeleted = IsDeleted.Delete;
                
                await _permissionQueryRepository.ChangeAsync(permission, cancellationToken);
            }

            #endregion
            
            #region HardDelete RoleUser

            var roleUsers = await _roleUserQueryRepository.FindAllByRoleIdAsync(@event.Id, cancellationToken);
            
            await _roleUserQueryRepository.RemoveRangeAsync(roleUsers, cancellationToken);

            #endregion
            
            #region HardDelete PermissionUser

            foreach (var permission in targetRole.Permissions)
            {
                var permissionUsers = await _permissionUserQueryRepository.FindAllByPermissionIdAsync(permission.Id, cancellationToken);
            
                await _permissionUserQueryRepository.RemoveRangeAsync(permissionUsers, cancellationToken);
            }

            #endregion
        }
    }

    public Task AfterHandleAsync(RoleDeleted @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}