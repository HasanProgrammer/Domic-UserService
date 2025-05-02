using Domic.Core.Common.ClassConsts;
using Domic.Core.Common.ClassEnums;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Events;

namespace Domic.UseCase.RoleUseCase.Events;

public class UpdateRoleConsumerEventBusHandler : IConsumerEventBusHandler<RoleUpdated>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public UpdateRoleConsumerEventBusHandler(IRoleQueryRepository roleQueryRepository) 
        => _roleQueryRepository = roleQueryRepository;

    public Task BeforeHandleAsync(RoleUpdated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Roles)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(RoleUpdated @event, CancellationToken cancellationToken)
    {
        var targetRole = await _roleQueryRepository.FindByIdAsync(@event.Id, cancellationToken);

        targetRole.Name        = @event.Name;
        targetRole.UpdatedBy   = @event.UpdatedBy;
        targetRole.UpdatedRole = @event.UpdatedRole;
        targetRole.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
        targetRole.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;

        await _roleQueryRepository.ChangeAsync(targetRole, cancellationToken);
    }

    public Task AfterHandleAsync(RoleUpdated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}