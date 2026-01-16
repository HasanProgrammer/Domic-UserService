using Domic.Core.Common.ClassConsts;
using Domic.Core.Common.ClassEnums;
using Domic.Core.Domain.Extensions;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class UpdateUserPasswordConsumerEventBusHandler(
    IUserQueryRepository userQueryRepository
) : IConsumerEventBusHandler<UserPasswordChanged>
{
    public Task BeforeHandleAsync(UserPasswordChanged @event, CancellationToken cancellationToken) 
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Users)]
    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(UserPasswordChanged @event, CancellationToken cancellationToken)
    {
        var targetUser = await userQueryRepository.FindByIdAsync(@event.Id, cancellationToken);
        
        targetUser.Password = await @event.NewPassword.HashAsync(cancellationToken);
        
        await userQueryRepository.ChangeAsync(targetUser, cancellationToken);
    }

    public Task AfterHandleAsync(UserPasswordChanged @event, CancellationToken cancellationToken) 
        => Task.CompletedTask;
}