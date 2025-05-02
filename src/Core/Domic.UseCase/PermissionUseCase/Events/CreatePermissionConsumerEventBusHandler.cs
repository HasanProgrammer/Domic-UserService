using Domic.Core.Common.ClassConsts;
using Domic.Core.Common.ClassEnums;
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

    public Task BeforeHandleAsync(PermissionCreated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Permissions)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(PermissionCreated @event, CancellationToken cancellationToken)
    {
        var targetPermission = await _permissionQueryRepository.FindByIdAsync(@event.Id, cancellationToken);

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
        
            await _permissionQueryRepository.AddAsync(newPermission, cancellationToken);
        }
    }

    public Task AfterHandleAsync(PermissionCreated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}