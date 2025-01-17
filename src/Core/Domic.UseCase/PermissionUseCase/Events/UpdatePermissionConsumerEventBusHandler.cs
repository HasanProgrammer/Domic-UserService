using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Events;

namespace Domic.UseCase.PermissionUseCase.Events;

public class UpdatePermissionConsumerEventBusHandler : IConsumerEventBusHandler<PermissionUpdated>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public UpdatePermissionConsumerEventBusHandler(IPermissionQueryRepository permissionQueryRepository) 
        =>  _permissionQueryRepository = permissionQueryRepository;

    public Task BeforeHandleAsync(PermissionUpdated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Permissions)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(PermissionUpdated @event, CancellationToken cancellationToken)
    {
        var targetPermission = await _permissionQueryRepository.FindByIdAsync(@event.Id, cancellationToken);

        targetPermission.Name   = @event.Name;
        targetPermission.RoleId = @event.RoleId;

        await _permissionQueryRepository.ChangeAsync(targetPermission, cancellationToken);
    }

    public Task AfterHandleAsync(PermissionUpdated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}