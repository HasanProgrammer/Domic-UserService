using Karami.Core.Domain.Enumerations;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Events;
using Karami.Domain.RoleUser.Contracts.Interfaces;

namespace Karami.UseCase.RoleUseCase.Events;

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

    [WithTransaction]
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
}