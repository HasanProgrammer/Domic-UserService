using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Events;
using Domic.Domain.PermissionUser.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Events;

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

    public Task BeforeHandleAsync(PermissionDeleted @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Permissions)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task Handle(PermissionDeleted @event, CancellationToken cancellationToken)
    {
        var targetPermission = await _permissionQueryRepository.FindByIdAsync(@event.Id, cancellationToken);

        if (targetPermission is not null) //Replication management
        {
            #region SoftDelete Permission

            targetPermission.IsDeleted = IsDeleted.Delete;
            
            await _permissionQueryRepository.ChangeAsync(targetPermission, cancellationToken);

            #endregion

            #region HardDelete PermissionUser
            
            var permissionUsers =
                await _permissionUserQueryRepository.FindAllByPermissionIdAsync(@event.Id, cancellationToken);
            
            await _permissionUserQueryRepository.RemoveRangeAsync(permissionUsers, cancellationToken);

            #endregion
        }
    }

    public Task AfterHandleAsync(PermissionDeleted @event, CancellationToken cancellationToken) => Task.CompletedTask;
}