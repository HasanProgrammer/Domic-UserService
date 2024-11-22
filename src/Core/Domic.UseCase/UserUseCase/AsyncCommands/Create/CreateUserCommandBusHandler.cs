using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.AsyncCommands.Create;

public class CreateUserCommandBusHandler : IConsumerCommandBusHandler<CreateUserCommandBus, string>
{
    public void BeforeHandle(CreateUserCommandBus message){}
    [WithValidation]
    [WithTransaction]
    public string Handle(CreateUserCommandBus message)
    {
        return "";
    }

    public void AfterHandle(CreateUserCommandBus message){}
}