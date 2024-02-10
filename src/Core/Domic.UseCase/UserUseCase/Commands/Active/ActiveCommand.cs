using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.Active;

public class ActiveCommand : ICommand<string>
{
    public required string Id    { get; set; }
    public required string Token { get; set; }
}