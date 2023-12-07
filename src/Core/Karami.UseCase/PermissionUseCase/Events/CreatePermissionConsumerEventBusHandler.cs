using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.Permission.Entities;
using Karami.Domain.Permission.Events;

namespace Karami.UseCase.PermissionUseCase.Events;

public class CreatePermissionConsumerEventBusHandler : IConsumerEventBusHandler<PermissionCreated>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public CreatePermissionConsumerEventBusHandler(IPermissionQueryRepository permissionQueryRepository) 
        => _permissionQueryRepository = permissionQueryRepository;

    [WithTransaction]
    public void Handle(PermissionCreated @event)
    {
        var targetPermission =
            _permissionQueryRepository.FindByIdAsync(@event.Id, default).Result;

        if (targetPermission is null) //Replication management
        {
            var newPermission = new PermissionQuery {
                Id     = @event.Id     ,
                RoleId = @event.RoleId ,
                Name   = @event.Name
            };
        
            _permissionQueryRepository.Add(newPermission);
        }
    }
}