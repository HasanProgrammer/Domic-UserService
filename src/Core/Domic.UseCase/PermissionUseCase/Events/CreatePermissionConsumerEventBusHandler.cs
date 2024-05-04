using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;
using Domic.Domain.Permission.Events;

namespace Domic.UseCase.PermissionUseCase.Events;

public class CreatePermissionConsumerEventBusHandler : IConsumerEventBusHandler<PermissionCreated>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public CreatePermissionConsumerEventBusHandler(IPermissionQueryRepository permissionQueryRepository) 
        => _permissionQueryRepository = permissionQueryRepository;

    [TransactionConfig(Type = TransactionType.Query)]
    public void Handle(PermissionCreated @event)
    {
        var targetPermission =
            _permissionQueryRepository.FindByIdAsync(@event.Id, default).Result;

        if (targetPermission is null) //Replication management
        {
            var newPermission = new PermissionQuery {
                Id          = @event.Id          ,
                RoleId      = @event.RoleId      ,
                Name        = @event.Name        ,
                CreatedBy   = @event.CreatedBy   ,
                CreatedRole = @event.CreatedRole ,
                CreatedAt_EnglishDate = @event.CreatedAt_EnglishDate,
                CreatedAt_PersianDate = @event.CreatedAt_PersianDate
            };
        
            _permissionQueryRepository.Add(newPermission);
        }
    }
}