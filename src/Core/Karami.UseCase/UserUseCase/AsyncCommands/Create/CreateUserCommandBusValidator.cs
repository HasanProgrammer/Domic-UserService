using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.AsyncCommands.Create;

public class CreateUserCommandBusValidator : IAsyncValidator<CreateUserCommandBus>
{
    public object Validate(CreateUserCommandBus input)
    {
        return default;
    }
}