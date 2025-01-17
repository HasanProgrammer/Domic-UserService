using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Domain.Role.Events;

namespace Domic.UseCase.RoleUseCase.Events;

public class CreateRoleConsumerEventBusHandler : IConsumerEventBusHandler<RoleCreated>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public CreateRoleConsumerEventBusHandler(IRoleQueryRepository roleQueryRepository) 
        => _roleQueryRepository = roleQueryRepository;

    public Task BeforeHandleAsync(RoleCreated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Roles)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(RoleCreated @event, CancellationToken cancellationToken)
    {
        var targetRole = await _roleQueryRepository.FindByIdAsync(@event.Id, cancellationToken);

        if (targetRole is null) //Replication management
        {
            var newRole = new RoleQuery {
                Id          =  @event.Id         ,
                Name        = @event.Name        ,
                CreatedBy   = @event.CreatedBy   ,
                CreatedRole = @event.CreatedRole ,
                CreatedAt_EnglishDate = @event.CreatedAt_EnglishDate,
                CreatedAt_PersianDate = @event.CreatedAt_PersianDate
            };

            await _roleQueryRepository.AddAsync(newRole, cancellationToken);
        }
    }

    public Task AfterHandleAsync(RoleCreated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}