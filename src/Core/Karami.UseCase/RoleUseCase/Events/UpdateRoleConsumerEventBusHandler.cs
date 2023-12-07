using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Events;

namespace Karami.UseCase.RoleUseCase.Events;

public class UpdateRoleConsumerEventBusHandler : IConsumerEventBusHandler<RoleUpdated>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public UpdateRoleConsumerEventBusHandler(IRoleQueryRepository roleQueryRepository) 
        => _roleQueryRepository = roleQueryRepository;

    [WithTransaction]
    public void Handle(RoleUpdated @event)
    {
        var targetRole = _roleQueryRepository.FindByIdAsync(@event.Id, default).Result;

        targetRole.Name = @event.Name;

        _roleQueryRepository.Change(targetRole);
    }
}