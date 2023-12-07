using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.Permission.Events;

namespace Karami.UseCase.PermissionUseCase.Events;

public class UpdatePermissionConsumerEventBusHandler : IConsumerEventBusHandler<PermissionUpdated>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public UpdatePermissionConsumerEventBusHandler(IPermissionQueryRepository permissionQueryRepository) 
        =>  _permissionQueryRepository = permissionQueryRepository;

    [WithTransaction]
    public void Handle(PermissionUpdated @event)
    {
        var targetPermission = _permissionQueryRepository.FindByIdAsync(@event.Id, default).Result;

        targetPermission.Name   = @event.Name;
        targetPermission.RoleId = @event.RoleId;

        _permissionQueryRepository.Change(targetPermission);
    }
}