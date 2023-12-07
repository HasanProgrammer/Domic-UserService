using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Commands.Active;

public class ActiveCommand : ICommand<string>
{
    public required string Id    { get; set; }
    public required string Token { get; set; }
}