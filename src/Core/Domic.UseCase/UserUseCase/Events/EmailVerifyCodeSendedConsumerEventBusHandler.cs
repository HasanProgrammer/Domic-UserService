using Domic.Core.Common.ClassEnums;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class EmailVerifyCodeSendedConsumerEventBusHandler(
    IUserCommandRepository userCommandRepository, IDateTime dateTime
    
) : IConsumerEventBusHandler<EmailVerifyCodeSended>
{
    public Task BeforeHandleAsync(EmailVerifyCodeSended @event, CancellationToken cancellationToken) 
        => Task.CompletedTask;

    [TransactionConfig(Type = TransactionType.Command)]
    public async Task HandleAsync(EmailVerifyCodeSended @event, CancellationToken cancellationToken)
    {
        var targetUser = await userCommandRepository.FindByEmailAsync(@event.EmailAddress, cancellationToken);
        
        targetUser.SetVerifyCode(dateTime, @event.CreatedBy, @event.CreatedRole, @event.VerifyCode);
        
        await userCommandRepository.ChangeAsync(targetUser, cancellationToken);
    }

    public Task AfterHandleAsync(EmailVerifyCodeSended @event, CancellationToken cancellationToken) 
        => Task.CompletedTask;
}