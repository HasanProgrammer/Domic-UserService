using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Entities;
using Karami.Domain.Role.Events;

namespace Karami.UseCase.RoleUseCase.Events;

public class CreateRoleConsumerEventBusHandler : IConsumerEventBusHandler<RoleCreated>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public CreateRoleConsumerEventBusHandler(IRoleQueryRepository roleQueryRepository) 
        => _roleQueryRepository = roleQueryRepository;

    [WithTransaction]
    public void Handle(RoleCreated @event)
    {
        var targetRole = _roleQueryRepository.FindByIdAsync(@event.Id, default).Result;

        if (targetRole is null) //Replication management
        {
            var newRole = new RoleQuery {
                Id   = @event.Id ,
                Name = @event.Name
            };

            _roleQueryRepository.Add(newRole);
        }
    }
}