using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class DeleteUserConsumerEventBusHandler : IConsumerEventBusHandler<UserDeleted>
{
    [TransactionConfig(Type = TransactionType.Query)]
    public void Handle(UserDeleted @event)
    {
        
    }

    public void AfterTransactionHandle(UserDeleted @event){}
}