using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.AsyncCommands.Create;

public class CreateUserCommandBusHandler : IConsumerCommandBusHandler<CreateUserCommandBus, string>
{
    [WithValidation]
    [WithTransaction]
    public string Handle(CreateUserCommandBus message)
    {
        return "";
    }
}