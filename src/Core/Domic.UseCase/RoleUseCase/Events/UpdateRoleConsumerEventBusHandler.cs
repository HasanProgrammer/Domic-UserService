using Domic.Core.Common.ClassConsts;
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

    [TransactionConfig(Type = TransactionType.Query)]
    public void Handle(RoleUpdated @event)
    {
        var targetRole = _roleQueryRepository.FindByIdAsync(@event.Id, default).Result;

        targetRole.Name = @event.Name;

        _roleQueryRepository.Change(targetRole);
    }
}