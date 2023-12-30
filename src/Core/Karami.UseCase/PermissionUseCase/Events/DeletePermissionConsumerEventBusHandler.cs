using System.Data;
using Karami.Core.Domain.Enumerations;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.Permission.Events;
using Karami.Domain.PermissionUser.Contracts.Interfaces;

namespace Karami.UseCase.PermissionUseCase.Events;

public class DeletePermissionConsumerEventBusHandler : IConsumerEventBusHandler<PermissionDeleted>
{
    private readonly IPermissionQueryRepository     _permissionQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;

    public DeletePermissionConsumerEventBusHandler(IPermissionQueryRepository permissionQueryRepository,
        IPermissionUserQueryRepository permissionUserQueryRepository
    )
    {
        _permissionQueryRepository     = permissionQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
    }

    [WithTransaction(IsolationLevel = IsolationLevel.ReadUncommitted)]
    public void Handle(PermissionDeleted @event)
    {
        var targetPermission = _permissionQueryRepository.FindByIdAsync(@event.Id, default).Result;

        if (targetPermission is not null) //Replication management
        {
            #region SoftDelete Permission

            targetPermission.IsDeleted = IsDeleted.Delete;
            
            _permissionQueryRepository.Change(targetPermission);

            #endregion

            #region HardDelete PermissionUser
            
            var permissionUsers =_permissionUserQueryRepository.FindAllByPermissionIdAsync(@event.Id, default).Result;
            
            _permissionUserQueryRepository.RemoveRange(permissionUsers);

            #endregion
        }
    }
}