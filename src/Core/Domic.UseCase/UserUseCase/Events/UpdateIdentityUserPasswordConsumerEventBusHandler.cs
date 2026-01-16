using Domic.Core.Common.ClassConsts;
using Domic.Core.Common.ClassEnums;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class UpdateIdentityUserPasswordConsumerEventBusHandler(
    IUserCommandRepository userCommandRepository, IDateTime dateTime
) : IConsumerEventBusHandler<IdentityUserPasswordChanged>
{
    public Task BeforeHandleAsync(IdentityUserPasswordChanged @event, CancellationToken cancellationToken) 
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Users)]
    [TransactionConfig(Type = TransactionType.Command)]
    public async Task HandleAsync(IdentityUserPasswordChanged @event, CancellationToken cancellationToken)
    {
        var targetUser = await userCommandRepository.FindByIdAsync(@event.Id, cancellationToken);
        
        targetUser.ResetPassword(dateTime, @event.NewPassword, @event.UpdatedBy, @event.UpdatedRole);
        
        await userCommandRepository.ChangeAsync(targetUser, cancellationToken);
    }

    public Task AfterHandleAsync(IdentityUserPasswordChanged @event, CancellationToken cancellationToken) 
        => Task.CompletedTask;
}