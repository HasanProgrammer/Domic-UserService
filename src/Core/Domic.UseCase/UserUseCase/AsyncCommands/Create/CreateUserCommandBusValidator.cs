using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.AsyncCommands.Create;

public class CreateUserCommandBusValidator : IAsyncValidator<CreateUserCommandBus>
{
    public object Validate(CreateUserCommandBus input)
    {
        return default;
    }
}