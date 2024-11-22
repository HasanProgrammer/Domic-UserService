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

    public void BeforeHandle(RoleDeleted @event){}

    [TransactionConfig(Type = TransactionType.Query)]
    public void Handle(RoleDeleted @event)
    {
        var targetRole = _roleQueryRepository.FindByIdEagerLoadingAsync(@event.Id, default).Result;

        if (targetRole is not null) //Replication management
        {
            #region SoftDelete Role

            targetRole.IsDeleted = IsDeleted.Delete;
            
            _roleQueryRepository.Change(targetRole);

            #endregion
        
            #region SoftDelete Permission

            foreach (var permission in targetRole.Permissions)
            {
                permission.IsDeleted = IsDeleted.Delete;
                
                _permissionQueryRepository.Change(permission);
            }

            #endregion
            
            #region HardDelete RoleUser

            var roleUsers = _roleUserQueryRepository.FindAllByRoleIdAsync(@event.Id, default).Result;
            
            _roleUserQueryRepository.RemoveRange(roleUsers);

            #endregion
            
            #region HardDelete PermissionUser

            foreach (var permission in targetRole.Permissions)
            {
                var permissionUsers = 
                    _permissionUserQueryRepository.FindAllByPermissionIdAsync(permission.Id, default).Result;
            
                _permissionUserQueryRepository.RemoveRange(permissionUsers);
            }

            #endregion
        }
    }

    public void AfterHandle(RoleDeleted @event){}
}